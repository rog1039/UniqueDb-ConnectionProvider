using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.Infrastructure
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this IList<T> list, IEnumerable<T> itemsToAdd)
        {
            foreach (var item in itemsToAdd)
            {
                list.Add(item);
            }
        }
    }
}