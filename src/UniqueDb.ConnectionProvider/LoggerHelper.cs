namespace UniqueDb.ConnectionProvider;

public static class LoggerHelper
{
    public static Action<string>           LogAction { get; set; } = Console.WriteLine;
    public static Action<Exception,string> LogError  { get; set; } = null;

    public static void Log(string      message)           => LogAction?.Invoke(message);
    public static void Error(Exception e, string message) => LogError?.Invoke(e, message);
}