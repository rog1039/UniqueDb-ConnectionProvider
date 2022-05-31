namespace UniqueDb.ConnectionProvider.Infrastructure.Extensions;

internal static class StringExtensions
{
   public static string[] SplitOn(this string text, params char[] splitChar)
   {
      return text.Split(splitChar, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
   }
}