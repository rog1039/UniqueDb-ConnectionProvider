using System;

namespace UniqueDb.ConnectionProvider
{
    public static class LoggerHelper
    {
        public static Action<string> LogAction { get; set; } = null;

        public static void Log(string message) => LogAction?.Invoke(message);
    }
}