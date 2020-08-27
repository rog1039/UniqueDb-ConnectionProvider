using System;
using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    internal static class ListExtensionMethods
    {
        internal static IEnumerable<T> Do<T>(this IEnumerable<T> list, Action<T> actionToPerform)
        {
            foreach (var item in list)
            {
                actionToPerform(item);
            }
            return list;
        }

        internal static IList<T> MakeList<T>(this T o)
        {
            IList<T> newList = new List<T>();
            newList.Add(o);
            return newList;
        }

        internal static string StringJoin(this IEnumerable<string> list, string separator)
        {
            return string.Join(separator, list);
        }
    }
}