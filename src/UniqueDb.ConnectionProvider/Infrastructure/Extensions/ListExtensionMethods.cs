namespace UniqueDb.ConnectionProvider.Infrastructure.Extensions;

public static class ListExtensionMethods
{
   internal static IEnumerable<T> Do<T>(this IEnumerable<T> list, Action<T, int> actionToPerform)
   {
      var enumerable = list.ToList();
      for (var index = 0; index < enumerable.Count; index++)
      {
         var item = enumerable[index];
         actionToPerform(item, index);
      }

      return enumerable;
   }

   internal static IEnumerable<T> Do<T>(this IEnumerable<T> list, Action<T> actionToPerform)
   {
      foreach (var item in list)
      {
         actionToPerform(item);
      }

      return list;
   }

   internal static IList<T> MakeList<T>(this T item)
   {
      return new List<T>() { item };
   }

   internal static string StringJoin(this IEnumerable<string> list, string separator)
   {
      return string.Join(separator, list);
   }

   /// <summary>
   /// The func allows one to transform the output of each string in the list.
   /// </summary>
   /// <param name="list"></param>
   /// <param name="separator"></param>
   /// <param name="func"></param>
   /// <returns></returns>
   internal static string StringJoin(this IEnumerable<string> list, string separator, Func<string, int, string> func)
   {
      list = list.Select((s, i) => func(s, i));
      return string.Join(separator, list);
   }
}