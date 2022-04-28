using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace UniqueDb.ConnectionProvider;

public interface ISqlConnectionProvider
{
    string DatabaseName { get; }
    string ServerName   { get; }

    bool   UseIntegratedAuthentication { get; }
    string UserName                    { get; }
    string Password                    { get; }

    SqlConnectionStringBuilder GetSqlConnectionStringBuilder();
    DbConnection               GetSqlConnection();
    SqlConnection              GetSqlConnectionWithTimeout(int timeout);
    string                     GetSqlConnectionString();

    string JustInstanceName { get; }
    string JustServerName   { get; }
}

public static class SqlConnectionProviderExtensions
{
    public static string ServerNameWithDbName(this ISqlConnectionProvider sqlConnectionProvider)
    {
        return $"{sqlConnectionProvider.ServerName}\\{sqlConnectionProvider.DatabaseName}";
    }

    public static SqlConnection ToSqlConnection(this ISqlConnectionProvider provider) =>
        provider.GetSqlConnection().ToSqlConnection();
}

public static class IDbConnectionExtensions
{
    public static SqlConnection ToSqlConnection(this IDbConnection connection) => (SqlConnection) connection;
}