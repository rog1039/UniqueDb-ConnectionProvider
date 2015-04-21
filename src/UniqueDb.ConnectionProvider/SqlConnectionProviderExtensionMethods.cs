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
    }
}