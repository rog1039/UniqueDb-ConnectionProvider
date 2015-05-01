using System;
using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public static class ListExtensionMethods
    {
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
    }
}