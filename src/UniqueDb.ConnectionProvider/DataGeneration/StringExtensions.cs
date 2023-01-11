using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public static class StringExtensions
{
   public static string Debracketize(this string input)
   {
      return input.Trim(new[] {'[', ']'});
   }

   public static string BracketizeSafe(this string input)
   {
      if (input[0] == '[' && input[^1] == ']') return input;
      return "[" + input + "]";
   }

   public static string Bracketize(this string input)
   {
      return "[" + input + "]";
   }

   public static string Bracify(this string input)
   {
      return "{" + input + "}";
   }

   public static string Repeat(this string input, int repeatCount)
   {
      StringBuilder sb = new StringBuilder();

      for (int i = 0; i < repeatCount; i++)
      {
         sb.Append(input);
      }

      return sb.ToString();
   }

   internal static bool InsensitiveEquals(this string s1, string s2)
   {
      var oneIsNullOtherIsnt = (s1 == null && s2 != null) || (s1 != null && s2 == null);
      if (oneIsNullOtherIsnt)
         return false;

      var bothAreNull = s1 == null && s2 == null;
      if (bothAreNull)
         return true;

      return s1.Length == s2.Length && s1.IndexOf(s2, StringComparison.InvariantCultureIgnoreCase) == 0;
   }

   public static bool UnDBInsensitiveContains(this string input, string part)
   {
      return CultureInfo.InvariantCulture.CompareInfo.IndexOf(input, part, CompareOptions.IgnoreCase) >= 0;
   }

   public static string ToUnderscoreCamelCase(this string s)
   {
      s = $"_" + s.ToCamelCase();
      return s;
   }

   public static string ToCamelCase(this string s)
   {
      return s[0].ToString().ToLower() + s.Substring(1);
   }

   public static (string FullName, string Name) ToNameParts(this string s)
   {
      var name     = s.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries).Last();
      var fullName = s.Trim();
      return (fullName, name);
   }

   internal static bool IsNullOrWhitespace([NotNullWhen(false)] this string? s)
   {
      return string.IsNullOrWhiteSpace(s);
   }

   internal static bool IsNotNullOrWhitespace([NotNullWhen(true)] this string? s)
   {
      return !string.IsNullOrWhiteSpace(s);
   }

   public static string RemoveEmptyLines(this string text)
   {
      //From: https://stackoverflow.com/a/7647762
      var resultString = Regex.Replace(text,
         @"^\s+$[\r\n]*",
         string.Empty,
         RegexOptions.Multiline);
      return resultString;
   }

   private static string SeparatorCommaNewLine = ", " + Environment.NewLine;

   public static string StringJoinCommaNewLine(this IEnumerable<string> list)
   {
      return string.Join(SeparatorCommaNewLine, list);
   }
}