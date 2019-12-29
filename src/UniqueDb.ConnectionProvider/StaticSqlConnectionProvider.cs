using System;
using Microsoft.Data.SqlClient;

namespace UniqueDb.ConnectionProvider
{
    public class StaticSqlConnectionProvider : BaseSqlConnectionProvider
    {
        private readonly bool _useIntegratedSecurity = true;

        public StaticSqlConnectionProvider(string serverName, string databaseName)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
        }

        private StaticSqlConnectionProvider(string serverName, string databaseName, string userName)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            UserName = userName;
        }

        public StaticSqlConnectionProvider(string serverName, string databaseName, string userName, string password)
            : this(serverName, databaseName)
        {
            _useIntegratedSecurity = false;
            UserName = userName;
            Password = password;
        }

        public StaticSqlConnectionProvider(string connectionString)
        {
            var parsedConnectionString = new SqlConnectionStringBuilder(connectionString);
            
            ServerName = parsedConnectionString.DataSource;
            DatabaseName = parsedConnectionString.InitialCatalog;

            var hasPassword = !string.IsNullOrWhiteSpace(parsedConnectionString.Password);
            if (hasPassword)
            {
                UserName = parsedConnectionString.UserID;
                Password = parsedConnectionString.Password;
                _useIntegratedSecurity = false;
            }
        }

        public override SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = ServerName;
            builder.InitialCatalog = DatabaseName;
            builder.IntegratedSecurity = _useIntegratedSecurity;
            if (!_useIntegratedSecurity)
            {
                builder.UserID = UserName;
                builder.Password = Password;
            }

            return builder;
        }

        public static ISqlConnectionProvider Blank => new StaticSqlConnectionProvider("", "");
    }
}