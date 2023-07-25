namespace UniqueDb.ConnectionProvider.Infrastructure.Extensions;

public static class ObjectExtensionMethods
{
    public static T As<T>(this object o)
    {
        return (T) o;
    }
}