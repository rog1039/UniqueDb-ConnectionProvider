using System;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class OptionExtensions
    {
        public static Option<T> Do<T>(this Option<T> source, Action<T> action)
        {
            if (source.HasValue)
            {
                action(source.Value);
            }
            return source;
        }
    }


}