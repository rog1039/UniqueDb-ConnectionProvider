using System.Collections;
using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.Tests.Infrastructure
{
    internal static class GenericExtensionMethods
    {
        public static IList<T> MakeList<T>(this T item)
        {
            return new List<T>(){item};
        }
    }
}