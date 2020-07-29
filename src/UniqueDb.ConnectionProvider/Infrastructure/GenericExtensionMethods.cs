using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.Infrastructure
{
    internal static class GenericExtensionMethods
    {
        public static IList<T> MakeList<T>(this T item)
        {
            return new List<T>(){item};
        }
    }
}