using System;

namespace UniqueDb.ConnectionProvider.Tests
{
    public static class BddStringExtensions
    {
        public static void _(this string s, Action action)
        {
            action();
        }
    }
}