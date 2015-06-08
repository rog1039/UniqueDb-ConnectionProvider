using System.Data.SqlClient;

namespace UniqueDb.ConnectionProvider
{
    public abstract class BaseSqlConnectionProvider : ISqlConnectionProvider
    {
        public string DatabaseName { get; protected set; }
        public string ServerName { get; protected set; }
        
        public virtual string GetSqlConnectionString() => GetSqlConnectionStringBuilder().ConnectionString;

        public abstract SqlConnectionStringBuilder GetSqlConnectionStringBuilder();
        
        public virtual SqlConnection GetSqlConnection() => new SqlConnection(GetSqlConnectionString());
    }
}