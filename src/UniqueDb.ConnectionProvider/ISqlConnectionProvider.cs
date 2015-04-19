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

    public abstract class BaseSqlConnectionProvider : ISqlConnectionProvider
    {
        public string DatabaseName { get; protected set; }
        public string ServerName { get; protected set; }
        
        public abstract SqlConnectionStringBuilder GetSqlConnectionStringBuilder();

        public virtual SqlConnection GetSqlConnection()
        {
            var sqlConnection = new SqlConnection(GetSqlConnectionString());
            return sqlConnection;
        }

        public virtual string GetSqlConnectionString()
        {
            var connectionString = GetSqlConnectionStringBuilder().ConnectionString;
            return connectionString;
        }
    }
}