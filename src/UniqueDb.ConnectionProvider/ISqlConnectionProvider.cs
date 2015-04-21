using System.Data.SqlClient;

namespace UniqueDb.ConnectionProvider
{
    public interface ISqlConnectionProvider
    {
        string DatabaseName { get; }
        string ServerName { get; }

        SqlConnectionStringBuilder GetSqlConnectionStringBuilder();
        SqlConnection GetSqlConnection();
        string GetSqlConnectionString();
    }
}