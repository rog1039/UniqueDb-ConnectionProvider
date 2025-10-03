using System.Text;
using System.Text.RegularExpressions;

namespace UniqueDb.ConnectionProvider;

public sealed class TableIdentifier
{
  public string Schema { get; }
  public string Table  { get; }

  public TableIdentifier(string schema, string table)
  {
    Schema = schema ?? throw new ArgumentNullException(nameof(schema));
    Table  = table ?? throw new ArgumentNullException(nameof(table));
    if (Schema.Length == 0 || Table.Length == 0)
      throw new ArgumentException("Schema and Table must be non-empty.");
  }

  public static TableIdentifier Parse(string input)
  {
    return TableIdentifierParser.Parse(input);
  }

  /// <summary>Returns [schema].[table] with proper escaping.</summary>
  public string ToSql() => $"{Quote(Schema)}.{Quote(Table)}";

  /// <summary>Escapes an identifier as [name] with ]] escaping.</summary>
  public static string Quote(string identifier)
  {
    if (identifier is null) throw new ArgumentNullException(nameof(identifier));
    return "[" + identifier.Replace("]", "]]") + "]";
  }

  public override string ToString() => ToSql();

  public void Deconstruct(out string schema, out string table)
  {
    schema = Schema;
    table  = Table;
  }

  /// <summary>Implicitly converts a string to a TableIdentifier using the default schema "dbo".</summary>
  // public static implicit operator TableIdentifier(string input)
  // {
  //   return TableIdentifierParser.Parse(input);
  // }
}

public static class TableIdentifierParser
{
  // Conservative validation for *unquoted* identifiers (quoted ones can contain almost anything)
  // Start: letter/underscore/@/# ; Rest: letters/digits/_/@/$/#
  private static readonly Regex UnquotedIdentifier =
    new Regex(@"^[A-Za-z_@#][A-Za-z0-9_@\$#]*$", RegexOptions.Compiled);

  /// <summary>
  /// Parse input like "MyTable" or "dbo.MyTable" or "[weird.schema].[my].[table]" (with dots/brackets inside).
  /// Returns a TableIdentifier; schema defaults to "dbo" if omitted.
  /// Throws ArgumentException on invalid input.
  /// </summary>
  public static TableIdentifier Parse(string input, string defaultSchema = "dbo")
  {
    if (string.IsNullOrWhiteSpace(input))
      throw new ArgumentException("Input cannot be null or whitespace.", nameof(input));

    var parts = SplitTopLevel(input.Trim());

    if (parts.Count == 1)
    {
      var table = ParsePart(parts[0]);
      return new TableIdentifier(defaultSchema, table);
    }
    else if (parts.Count == 2)
    {
      var schema = ParsePart(parts[0]);
      var table  = ParsePart(parts[1]);
      return new TableIdentifier(schema, table);
    }
    else
    {
      // Intentionally reject database/server-part names to keep scope to schema.table
      throw new ArgumentException("Only one- or two-part names are allowed (table or schema.table).");
    }
  }

  /// <summary>TryParse variant.</summary>
  public static bool TryParse(string input, out TableIdentifier? result, string defaultSchema = "dbo")
  {
    try
    {
      result = Parse(input, defaultSchema);
      return true;
    }
    catch
    {
      result = null;
      return false;
    }
  }

  // ---- helpers ----

  private static string ParsePart(Token token)
  {
    // Bracketed part: already unescaped during tokenization
    if (token.IsBracketed)
    {
      if (token.Value.Length == 0)
        throw new ArgumentException("Empty bracketed identifier is not allowed.");
      return token.Value;
    }

    // Unquoted part: validate strictly to avoid injection via special chars
    var trimmed = token.Value.Trim();
    if (!UnquotedIdentifier.IsMatch(trimmed))
      throw new ArgumentException($"Invalid unquoted identifier: \"{trimmed}\"");
    return trimmed;
  }

  private sealed class Token
  {
    public string Value       { get; }
    public bool   IsBracketed { get; }

    public Token(string value, bool bracketed)
    {
      Value       = value;
      IsBracketed = bracketed;
    }

    public override string ToString() => IsBracketed ? $"[{Value}]" : Value;
  }

  /// <summary>
  /// Splits on '.' at top level only (ignoring dots inside [brackets]),
  /// trims outer whitespace, supports escaped closing bracket (]]).
  /// Also unescapes ]] to ] for bracketed tokens.
  /// </summary>
  private static List<Token> SplitTopLevel(string s)
  {
    var  result         = new List<Token>(2);
    var  sb             = new StringBuilder();
    bool inBrackets     = false;
    bool tokenBracketed = false;

    // When we just finished a bracketed token, we only allow whitespace then '.' or end-of-input
    bool awaitingSeparatorAfterBracket = false;

    void pushToken()
    {
      var text = sb.ToString();
      sb.Clear();

      if (!tokenBracketed)
        text = text.Trim();

      result.Add(new Token(text, tokenBracketed));
      tokenBracketed                = false;
      awaitingSeparatorAfterBracket = false;
    }

    for (int i = 0; i < s.Length; i++)
    {
      char c = s[i];

      // If we just closed a bracketed token, enforce boundary rules
      if (awaitingSeparatorAfterBracket)
      {
        if (char.IsWhiteSpace(c))
        {
          // ignore whitespace between parts
          continue;
        }

        if (c == '.')
        {
          // boundary confirmed; complete previous token and start a new one
          pushToken();
          continue;
        }

        // Any other char is invalid (e.g., []?J or [abc]xyz)
        throw new ArgumentException("Invalid characters after a bracketed identifier. Expect '.' or end of input.");
      }

      if (!inBrackets)
      {
        if (c == '.')
        {
          // split current token
          pushToken();
          continue;
        }

        if (c == '[')
        {
          // Starting a bracketed token is only valid if we haven't already collected
          // any non-whitespace characters in the current token buffer.
          if (sb.Length == 0 || string.IsNullOrWhiteSpace(sb.ToString()))
          {
            inBrackets     = true;
            tokenBracketed = true;
            sb.Clear(); // discard any whitespace gathered so far
            continue;
          }

          // '[' encountered mid unquoted-segment -> invalid
          throw new ArgumentException("Invalid '[' in unquoted identifier.");
        }

        if (c == ']')
        {
          // stray ']' outside of brackets
          throw new ArgumentException("Unmatched ']' in identifier.");
        }

        sb.Append(c);
        continue;
      }
      else
      {
        // Inside brackets
        if (c == ']')
        {
          // Escaped ']]' => literal ']'
          if (i + 1 < s.Length && s[i + 1] == ']')
          {
            sb.Append(']');
            i++; // consume the escape
            continue;
          }

          // Close the bracketed token; do not push yet — wait for '.' or end
          inBrackets                    = false;
          awaitingSeparatorAfterBracket = true;
          continue;
        }

        sb.Append(c);
      }
    }

    if (inBrackets)
      throw new ArgumentException("Unterminated bracketed identifier.");

    // If we just closed a bracket and reached end-of-input, push that token now
    if (awaitingSeparatorAfterBracket)
      pushToken();
    else
    {
      // If anything remains in buffer (including unquoted), push it
      if (sb.Length > 0 || s.EndsWith(".", StringComparison.Ordinal))
        pushToken();
    }

    // Validate no empty identifiers
    foreach (var t in result)
    {
      if (t.Value.Length == 0)
        throw new ArgumentException("Empty identifier part is not allowed.");
    }

    return result;
  }
}