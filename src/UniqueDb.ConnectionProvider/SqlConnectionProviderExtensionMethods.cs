using System;
using System.Data.SqlClient;

namespace UniqueDb.ConnectionProvider
{
    public static class SqlConnectionProviderExtensionMethods
    {
        public static SqlConnectionDbDisposable ToDisposable(this ISqlConnectionProvider dbConnectionProvider)
        {
            var disposable = new SqlConnectionDbDisposable(dbConnectionProvider);
            return disposable;
        }

        public static SqlConnection ConnectionAsMaster(this ISqlConnectionProvider dbConnectionProvider)
        {
            var sqlConnectionStringBuilder = dbConnectionProvider.GetSqlConnectionStringBuilder();
            sqlConnectionStringBuilder.InitialCatalog = "master";
            return new SqlConnection(sqlConnectionStringBuilder.ConnectionString);
        }

        public static UniqueDbConnectionProvider AndAutoDeleteDbOlderThan5Minutes(this UniqueDbConnectionProvider uniqueDbConnectionProvider)
        {
            OldDatabaseDeleter.DeleteOldDatabases(uniqueDbConnectionProvider, TimeSpan.FromMinutes(5));
            return uniqueDbConnectionProvider;
        }

        public static UniqueDbConnectionProvider AndAutoDeleteDbOlderThan(this UniqueDbConnectionProvider uniqueDbConnectionProvider, TimeSpan olderThan)
        {
            OldDatabaseDeleter.DeleteOldDatabases(uniqueDbConnectionProvider, olderThan);
            return uniqueDbConnectionProvider;
        }

        public static void CreateDatabase(this ISqlConnectionProvider connectionProvider)
        {
            var connection = CreateSqlConnectionOnMasterDatabase(connectionProvider);

            var createDbText = string.Format("CREATE DATABASE [{0}];", connectionProvider.DatabaseName);
            Console.WriteLine(createDbText);

            var command = new SqlCommand(createDbText, connection);
            command.ExecuteNonQuery();
            connection.Dispose();
        }

        private static SqlConnection CreateSqlConnectionOnMasterDatabase(ISqlConnectionProvider connectionProvider)
        {
            var connectionStringBuilder = connectionProvider.GetSqlConnectionStringBuilder();
            connectionStringBuilder.InitialCatalog = "master";
            var connectionString = connectionStringBuilder.ConnectionString;
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }

        public static void Execute(this ISqlConnectionProvider connectionProvider, string sqlCommand)
        {
            var connection = connectionProvider.GetSqlConnection();
            connection.Open();
            var command = new SqlCommand(sqlCommand, connection);
            command.ExecuteNonQuery();
            connection.Dispose();
        }

        public static T ExecuteScalar<T>(this ISqlConnectionProvider connectionProvider, string sqlCommand)
        {
            var connection = connectionProvider.GetSqlConnection();
            connection.Open();
            var command = new SqlCommand(sqlCommand, connection);
            var result = (T)command.ExecuteScalar();
            connection.Dispose();
            return result;
        }
    }
}