using System;
using System.Data.SqlClient;
using Xbehave;

namespace UniqueDb.ConnectionProvider.Tests
{
    public class DatabaseDeletionTests
    {
        [Scenario]
        public void ShouldDispose()
        {
            var options = new UniqueDbConnectionProviderOptions("ws2012sqlexp1\\sqlexpress", "autodisposedatabase");
            var connectionProvider = new UniqueDbConnectionProvider(options);

            "After creating a database"
                ._(() => CreateDatabase(connectionProvider));

            "Disposing of the disposable provided by the ToDisposable extension method should delete the database"
                ._(() =>
                {
                    using (var lifecycle = connectionProvider.ToDispopsable())
                    {
                    }
                });
        }

        private void CreateDatabase(UniqueDbConnectionProvider connectionProvider)
        {
            var connection = CreateSqlConnectionOnMasterDatabase(connectionProvider);

            var createDbText = string.Format("CREATE DATABASE [{0}];", connectionProvider.DatabaseName);
            Console.WriteLine(createDbText);

            var command = new SqlCommand(createDbText, connection);
            command.ExecuteNonQuery();
            connection.Dispose();
        }

        private static SqlConnection CreateSqlConnectionOnMasterDatabase(UniqueDbConnectionProvider connectionProvider)
        {
            var connectionStringBuilder = connectionProvider.GetSqlConnectionStringBuilder();
            connectionStringBuilder.InitialCatalog = "master";
            var connectionString = connectionStringBuilder.ConnectionString;
            var connection = new SqlConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}