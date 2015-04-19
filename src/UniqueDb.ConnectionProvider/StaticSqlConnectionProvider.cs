using System;
using System.Data.SqlClient;

namespace UniqueDb.ConnectionProvider
{
    public class StaticSqlConnectionProvider : BaseSqlConnectionProvider
    {
        private readonly string _userName;
        private readonly string _password;
        private Lazy<SqlConnectionStringBuilder> sqlConnectionStringBuilder;
        private bool _useIntegratedSecurity = true;
        
        public StaticSqlConnectionProvider(string serverName, string databaseName)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            sqlConnectionStringBuilder = new Lazy<SqlConnectionStringBuilder>(CreateSqlConnectionStringBuilder);
        }

        public StaticSqlConnectionProvider(string serverName, string databaseName, string userName, string password)
            : this(serverName, databaseName)
        {
            _useIntegratedSecurity = false;
            _userName = userName;
            _password = password;
        }

        public override SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
        {
            return sqlConnectionStringBuilder.Value;
        }

        private SqlConnectionStringBuilder CreateSqlConnectionStringBuilder()
        {
            var builder = new SqlConnectionStringBuilder();
            builder.DataSource = ServerName;
            builder.InitialCatalog = base.DatabaseName;
            builder.IntegratedSecurity = _useIntegratedSecurity;
            if (!_useIntegratedSecurity)
            {
                builder.UserID = _userName;
                builder.Password = _password;
            }
            return builder;
        }
    }
}