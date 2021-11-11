using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider;

public static class CollectionExtensions
{
    internal static void AddRange<T>(this IList<T> list, IEnumerable<T> itemsToAdd)
    {
        foreach (var item in itemsToAdd)
        {
            list.Add(item);
        }
    }
}