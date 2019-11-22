using System;
using System.Data.SqlClient;

namespace UniqueDb.ConnectionProvider
{
    public class StaticSqlConnectionProvider : BaseSqlConnectionProvider
    {
        private readonly string _userName;
        private readonly string _password;
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
            _userName = userName;
        }

        public StaticSqlConnectionProvider(string serverName, string databaseName, string userName, string password)
            : this(serverName, databaseName)
        {
            _useIntegratedSecurity = false;
            _userName = userName;
            _password = password;
        }

        public StaticSqlConnectionProvider(string connectionString)
        {
            var parsedConnectionString = new SqlConnectionStringBuilder(connectionString);
            
            ServerName = parsedConnectionString.DataSource;
            DatabaseName = parsedConnectionString.InitialCatalog;

            var hasPassword = !string.IsNullOrWhiteSpace(parsedConnectionString.Password);
            if (hasPassword)
            {
                _userName = parsedConnectionString.UserID;
                _password = parsedConnectionString.Password;
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
                builder.UserID = _userName;
                builder.Password = _password;
            }

            return builder;
        }

        public static ISqlConnectionProvider Blank => new StaticSqlConnectionProvider("", "");
    }
}