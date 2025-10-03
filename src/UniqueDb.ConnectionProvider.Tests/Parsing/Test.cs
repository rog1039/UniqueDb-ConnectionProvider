using System.Text;
using NUnit.Framework;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using Xunit;
using Assert = Xunit.Assert;

namespace UniqueDb.ConnectionProvider.Tests.Parsing;

public class TableIdentifierParserTests
{
    // ---------- Valid cases ----------

    [Fact]
    public void Parse_UnqualifiedName_DefaultsToDbo()
    {
        var id = TableIdentifierParser.Parse("Orders");
        Assert.Equal("dbo",            id.Schema);
        Assert.Equal("Orders",         id.Table);
        Assert.Equal("[dbo].[Orders]", id.ToSql());
    }

    [Fact]
    public void Parse_SchemaAndTable_Unquoted()
    {
        var id = TableIdentifierParser.Parse("sales.Orders");
        Assert.Equal("sales",            id.Schema);
        Assert.Equal("Orders",           id.Table);
        Assert.Equal("[sales].[Orders]", id.ToSql());
    }

    [Fact]
    public void Parse_BracketedIdentifiers_WithDotInside()
    {
        var id = TableIdentifierParser.Parse("[sales].[2024.Q4]");
        Assert.Equal("sales",             id.Schema);
        Assert.Equal("2024.Q4",           id.Table);
        Assert.Equal("[sales].[2024.Q4]", id.ToSql());
    }

    [Fact]
    public void Parse_BracketedIdentifier_EscapedBracket()
    {
        // Input contains an escaped closing bracket pair "]]" inside bracketed identifier
        // e.g. [weird]]name] -> value stored = "weird]name"
        var id = TableIdentifierParser.Parse("[weird]]name]");
        Assert.Equal("dbo",        id.Schema);
        Assert.Equal("weird]name", id.Table);

        // Quote should re-escape the ']' back to ']]'
        Assert.Equal("[weird]]name]",       TableIdentifier.Quote(id.Table));
        Assert.Equal("[dbo].[weird]]name]", id.ToSql());
    }

    [Fact]
    public void TryParse_ValidExamples_ReturnsTrue()
    {
        Assert.True(TableIdentifierParser.TryParse("Orders",         out var a) && a != null);
        Assert.True(TableIdentifierParser.TryParse("dbo.Orders",     out var b) && b != null);
        Assert.True(TableIdentifierParser.TryParse("[dbo].[Orders]", out var c) && c != null);
    }

    // ---------- Invalid cases (Parse should throw) ----------

    [Xunit.Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Parse_NullOrWhiteSpace_ThrowsArgumentException(string input)
    {
        Assert.Throws<ArgumentException>(() => TableIdentifierParser.Parse(input));
    }

    [Fact]
    public void Parse_TooManyParts_ThrowsArgumentException()
    {
        // database.schema.table -> rejected by design
        Assert.Throws<ArgumentException>(() => TableIdentifierParser.Parse("mydb.dbo.Orders"));
    }

    [Fact]
    public void Parse_UnterminatedBracket_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() => TableIdentifierParser.Parse("[unclosed"));
    }

    [Fact]
    public void Parse_EmptyPart_ThrowsArgumentException()
    {
        // trailing dot "dbo." or leading ".Users" should be rejected
        Assert.Throws<ArgumentException>(() => TableIdentifierParser.Parse("dbo."));
        Assert.Throws<ArgumentException>(() => TableIdentifierParser.Parse(".Users"));
    }

    [Fact]
    public void Parse_InvalidUnquotedIdentifier_ThrowsArgumentException()
    {
        // contains semicolon
        Assert.Throws<ArgumentException>(() => TableIdentifierParser.Parse("sa;les.Orders"));

        // starts with a digit
        Assert.Throws<ArgumentException>(() => TableIdentifierParser.Parse("1table"));
    }

    // ---------- TryParse should return false for malformed and injection-like inputs ----------

    [Xunit.Theory]
    [InlineData("dbo; DROP TABLE Users")]
    [InlineData("dbo.Orders;--")]
    [InlineData("dbo.Orders; EXEC xp_cmdshell 'dir'")]
    [InlineData("dbo..Orders")]
    [InlineData("dbo.Orders; DROP DATABASE master")]
    [InlineData("dbo; EXEC sp_msforeachdb 'DROP DATABASE ?'")]
    [InlineData("DROP TABLE Orders")]
    [InlineData("DROP TABLE")]
    public void TryParse_SqlInjectionLikeInputs_ReturnsFalse(string input)
    {
        Assert.False(TableIdentifierParser.TryParse(input, out var _));
    }

    [Test]
    public void TestDropTable()
    {
        //This is OK because TableIdentifier will just quote the names.
        var result = TableIdentifierParser.TryParse("Drop.Table", out var id);
        Assert.Equal("Drop",           id.Schema);
        Assert.Equal("Table",          id.Table);
        Assert.Equal("[Drop].[Table]", id.ToString());
        Assert.Equal("[Drop].[Table]", id.ToSql());
        id.ToSql().ToConsole();
    }

    [Fact]
    public void TryParse_UnterminatedBracket_ReturnsFalse()
    {
        Assert.False(TableIdentifierParser.TryParse("[abc", out _));
    }

    [Fact]
    public void TryParse_TooManyParts_ReturnsFalse()
    {
        Assert.False(TableIdentifierParser.TryParse("a.b.c", out _));
    }

    // ---------- Additional behavior tests ----------

    [Fact]
    public void Quote_EscapesClosingBracket()
    {
        var original = "x]y]";
        var quoted   = TableIdentifier.Quote(original);
        // each ']' becomes ']]' inside quoted form
        Assert.Equal("[x]]y]]]", quoted);
    }

    [Fact]
    public void Parse_BracketWithLeadingWhitespaceInside_Allowed()
    {
        // bracketed preserves internal whitespace
        var id = TableIdentifierParser.Parse("[  weird name  ]");
        Assert.Equal("  weird name  ",         id.Table);
        Assert.Equal("[dbo].[  weird name  ]", id.ToSql());
    }

    [TestCase("ab[cd.ef")]
    [TestCase("abcd.e[f")]
    [TestCase("[abcd.ef")]
    [TestCase("abcd.[ef")]
    [TestCase("table[name")]
    [TestCase("[tablename")]
    [TestCase("tablename[")]
    public void Parse_RejectsOpenBracket(string input)
    {
        // e.g. unquoted token containing '[' should be invalid
        Assert.Throws<ArgumentException>(() => TableIdentifierParser.Parse(input));
    }
    
    [Test]
    public void Parse_Rejects_EmptyBracketThenChars()
    {
        Assert.False(TableIdentifierParser.TryParse("[]?J", out _));
        Assert.Throws<ArgumentException>(() => TableIdentifierParser.Parse("[]?J"));
    }

    [TestCase("[abc]xyz")]
    [TestCase("[abc]  xyz")]
    [TestCase("[abc]  ?")]
    [TestCase("[abc]  [def]")]
    public void Parse_Rejects_TrailingCharsAfterBracketedToken(string input)
    {
        Assert.False(TableIdentifierParser.TryParse(input, out _));
        Assert.Throws<ArgumentException>(() => TableIdentifierParser.Parse(input));
    }

    [TestCase("[abc].[def]")]
    [TestCase("[abc]   .   [def]")]
    [TestCase(" abc  .  [def]")]
    public void Parse_Allows_WhitespaceAroundDotSeparators(string input)
    {
        var id = TableIdentifierParser.Parse(input);
        Assert.Equal("abc", id.Schema);
        Assert.Equal("def", id.Table);
    }
}
public class TableNameParserFuzzTests
{
    private static readonly char[] FuzzChars =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 []().;'-_@$%*!?\\/\":,[]".ToCharArray();

    [Test]
    public void Fuzz_RandomInputs_RoundTripSafety()
    {
        const int iterations = 50000;
        const int maxLen     = 80;
        const int seed       = 123456; // reproducible

        var rnd = new Random(seed);

        for (int i = 0; i < iterations; i++)
        {
            int len = rnd.Next(0, maxLen + 1);
            var sb  = new StringBuilder(len);
            for (int j = 0; j < len; j++)
            {
                // bias toward generating bracket characters occasionally
                if (rnd.NextDouble() < 0.06)
                    sb.Append('[');
                else if (rnd.NextDouble() < 0.06)
                    sb.Append(']');
                else if (rnd.NextDouble() < 0.06)
                    sb.Append('.');
                else
                    sb.Append(FuzzChars[rnd.Next(FuzzChars.Length)]);
            }

            var candidate = sb.ToString();

            // We expect TryParse never to throw. It returns true/false.
            bool ok = TableIdentifierParser.TryParse(candidate, out var result);

            // If it parsed successfully, assert invariant properties about the produced identifier
            if (ok)
            {
                Assert.NotNull(result);
                Assert.False(string.IsNullOrWhiteSpace(result.Schema), "Parsed schema is empty");
                Assert.False(string.IsNullOrWhiteSpace(result.Table),  "Parsed table is empty");

                // ToSql should be constructed from proper quoting
                var expectedToSql = $"{TableIdentifier.Quote(result.Schema)}.{TableIdentifier.Quote(result.Table)}";
                Assert.Equal(expectedToSql, result.ToSql());

                // Verify Quote round-trip: removing outer brackets and unescaping "]]" should equal original value
                string UnescapeQuoted(string quoted)
                {
                    Assert.True(quoted.StartsWith("["), "quoted must start with [");
                    Assert.True(quoted.EndsWith("]"),   "quoted must end with ]");
                    var inner = quoted.Substring(1, quoted.Length - 2);
                    return inner.Replace("]]", "]");
                }

                var schemaQuoted = TableIdentifier.Quote(result.Schema);
                var tableQuoted  = TableIdentifier.Quote(result.Table);

                Assert.Equal(result.Schema, UnescapeQuoted(schemaQuoted));
                Assert.Equal(result.Table,  UnescapeQuoted(tableQuoted));
            }
            else
            {
                // ok = false indicates input rejected — nothing to assert
            }
            if(ok) Console.WriteLine($"Result: {ok,6} - {candidate} ||| {result?.ToSql()}");
        }
    }

    [Fact]
    public void Fuzz_KnownInjectionAndEdgeSeeds()
    {
        var seeds = new[]
        {
            // common SQL injection attempts
            "dbo; DROP TABLE Users",
            "dbo.Orders;--",
            "dbo.Orders; EXEC xp_cmdshell 'dir'",
            "dbo..Orders",
            "dbo.Orders; DROP DATABASE master",
            "dbo; EXEC sp_msforeachdb 'DROP DATABASE ?'",
            "mydb.dbo.Orders", // intentionally rejected because we only allow schema.table (two parts)
            "[unclosed",
            "[]",  // empty bracketed identifier
            "[ ]", // brackets with only whitespace -> parser currently allows inside bracket but up to you
            ".Users",
            "dbo.",
            "a.b.c",
            "name with spaces", // unquoted containing spaces -> should be rejected
            "name;select 1",    // semicolon present
            "name--comment",    // comment sequence
            "name/*comment*/",  // comment token
            "name; DROP TABLE t; --",
            "sa; --",
            "][",                  // inverted brackets
            "[]].[]",              // malformed bracket escape
            "normal].[evil",       // bracket in middle unquoted
            "schema.[table].extra" // three part, bracketed but too many parts
        };

        foreach (var s in seeds)
        {
            bool ok = TableIdentifierParser.TryParse(s, out var result);

            // For many of these we expect TryParse to be false (rejected).
            // Some borderline cases (e.g., "[ ]") may be allowed by design — adjust expected behavior if you intend otherwise.
            // We'll assert false for obviously malicious/malformed inputs from the list above.
            Assert.False(ok, $"Seed input unexpectedly parsed as valid: \"{s}\". Parsed -> {result?.ToSql() ?? "<null>"}");
        }
    }
}