namespace UniqueDb.ConnectionProvider.Infrastructure.Extensions;

public static class SqlTemplateExtensions
{
   public static string MyReplace(this string template, string key, string input)
   {
      return template.Replace($"${key}$", input);
   }
   public static string MyReplace2(this string template, string key, string input)
   {
      return template.Replace($"${key}", input);
   }
}