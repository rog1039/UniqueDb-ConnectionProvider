namespace Woeber.Logistics.FluentDbMigrations.Tests;

public static class SqlTemplateExtensions
{
   public static string MyReplace(this string template, string key, string input)
   {
      return template.Replace($"${key}$", input);
   }
}