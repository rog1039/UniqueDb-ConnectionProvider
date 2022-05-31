namespace UniqueDb.ConnectionProvider.Infrastructure.Extensions;

internal static class GenericExtensionMethods
{
   public static IList<T> MakeList<T>(this T item)
   {
      return new List<T>() {item};
   }
}