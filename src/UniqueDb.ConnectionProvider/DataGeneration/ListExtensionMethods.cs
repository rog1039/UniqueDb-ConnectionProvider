using System;
using System.Collections.Generic;
using System.Linq;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class ListExtensionMethods
    {
        public static IEnumerable<T> Do<T>(this IEnumerable<T> list, Action<T, int> actionToPerform)
        {
            var enumerable = list.ToList();
            for (var index = 0; index < enumerable.Count; index++)
            {
                var item = enumerable[index];
                actionToPerform(item, index);
            }

            return enumerable;
        }
        public static IEnumerable<T> Do<T>(this IEnumerable<T> list, Action<T> actionToPerform)
        {
            foreach (var item in list)
            {
                actionToPerform(item);
            }
            return list;
        }

        public static IList<T> MakeList<T>(this T o)
        {
            IList<T> newList = new List<T>();
            newList.Add(o);
            return newList;
        }

        public static string StringJoin(this IEnumerable<string> list, string separator)
        {
            return string.Join(separator, list);
        }
    }
}