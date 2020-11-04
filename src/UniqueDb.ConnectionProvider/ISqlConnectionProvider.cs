using Microsoft.Data.SqlClient;

namespace UniqueDb.ConnectionProvider
{
    public interface ISqlConnectionProvider
    {
        string DatabaseName { get; }
        string ServerName { get; }

        bool UseIntegratedAuthentication { get; }
        string UserName { get; }
        string Password { get; }

        SqlConnectionStringBuilder GetSqlConnectionStringBuilder();
        SqlConnection GetSqlConnection();
        string GetSqlConnectionString();

        string JustInstanceName { get; }
        string JustServerName { get; }
    }

    public static class SqlConnectionProviderExtensions
    {
        public static string ServerNameWithDbName(this ISqlConnectionProvider sqlConnectionProvider)
        {
            return $"{sqlConnectionProvider.ServerName}.{sqlConnectionProvider.DatabaseName}";
        }
    }
}