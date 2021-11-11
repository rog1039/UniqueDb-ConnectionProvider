namespace UniqueDb.ConnectionProvider.Tests;

public static class BddStringExtensions
{
    public static void _(this string s, Action action)
    {
        action();
    }

    public static void _foreach<T>(this string s, IList<T> collection, Action<T> action)
    {
        foreach (var item in collection)
        {
            action(item);
        }
    }
}