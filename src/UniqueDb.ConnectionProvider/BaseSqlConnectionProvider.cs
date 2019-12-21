using Microsoft.Data.SqlClient;

namespace UniqueDb.ConnectionProvider
{
    public abstract class BaseSqlConnectionProvider : ISqlConnectionProvider
    {
        public string DatabaseName { get; protected set; }
        public string ServerName { get; protected set; }

        public bool UseIntegratedAuthentication { get; protected set; }
        public string UserName { get; protected set; }
        public string Password { get; protected set; }

        public virtual string GetSqlConnectionString() => GetSqlConnectionStringBuilder().ConnectionString;
        
        public abstract SqlConnectionStringBuilder GetSqlConnectionStringBuilder();
        
        public virtual SqlConnection GetSqlConnection() => new SqlConnection(GetSqlConnectionString());

        public string JustInstanceName => GetInstanceName();

        private string GetInstanceName()
        {
            return MyStringUtils.EndTo(ServerName, "\\");
        }

        public string JustServerName => GetServerName();

        private string GetServerName()
        {
            return MyStringUtils.StartTo(ServerName, "\\");
        }
    }
}