using System;
using System.Linq;
using Dapper;
using Microsoft.Data.SqlClient;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;

namespace UniqueDb.ConnectionProvider
{
    public static class SqlConnectionProviderExtensionMethods
    {
        public static SqlConnectionDbDeletingDisposable ToSelfDeletingDisposable(this ISqlConnectionProvider dbConnectionProvider)
        {
            var disposable = new SqlConnectionDbDeletingDisposable(dbConnectionProvider);
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

        public static void EnsureDatabaseExists(this ISqlConnectionProvider connectionProvider)
        {
            var databaseExists = DoesDatabaseExist(connectionProvider);
            if (!databaseExists) connectionProvider.CreateDatabase();
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

        public static bool DoesDatabaseExist(this ISqlConnectionProvider connectionProvider, string databaseName = null)
        {
            databaseName = databaseName ?? connectionProvider.DatabaseName;
            var connection = CreateSqlConnectionOnMasterDatabase(connectionProvider);
            var doesDatabaseExistTextQuery = $"SELECT 1 WHERE db_id('{databaseName}') IS NOT NULL";
            var doesDatabaseExist = connection
                .Query<int>(doesDatabaseExistTextQuery)
                .Any();
            return doesDatabaseExist;
        }

        public static void ExecuteNonDapper(this ISqlConnectionProvider connectionProvider, string sqlCommand)
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
            var result = ConvertFromDBVal<T>(command.ExecuteScalar());
            connection.Dispose();
            return result;
        }
        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }

        public static string GenerateClassFromQuery(this ISqlConnectionProvider sqlConnectionProvider, string sqlQuery,
            string className)
        {
            return CSharpClassGeneratorFromQueryViaSqlDescribeResultSet.GenerateClass(sqlConnectionProvider, sqlQuery, className);
        }
        
        public static string GenerateClassFromTable(this ISqlConnectionProvider sqlConnectionProvider, string schemaname, string tableName,
            string className = null)
        {
            className = className ?? tableName;
            var sqlTableReference = new SqlTableReference(sqlConnectionProvider, schemaname, tableName);
            var sqlTable = SqlTableFactory.Create(sqlTableReference);
            return CSharpClassGeneratorFromSqlTable.GenerateClass(sqlTable, className);
        }

        public static ISqlConnectionProvider ChangeDatabase(this ISqlConnectionProvider sqlConnectionProvider, string databaseName)
        {
            var oldScp = sqlConnectionProvider;
            var newScp = new StaticSqlConnectionProvider(oldScp.ServerName, databaseName, 
                                                         oldScp.UserName, oldScp.Password);
            return newScp;
        }
    }
}