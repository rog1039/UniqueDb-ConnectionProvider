using System;
using System.Data.SqlClient;
using Xbehave;

namespace UniqueDb.ConnectionProvider.Tests
{
    public class ConnectionProviderTests
    {
        [Scenario]
        public void GetConnectionString()
        {
            var options = new UniqueDbConnectionProviderOptions("server", "database");
            var connectionProvider = new UniqueDbConnectionProvider(options);

            var connectionString = connectionProvider.GetSqlConnectionString();
            Console.WriteLine(connectionString);
        }

        [Scenario]
        public void GetConnectionString_WithUserNameAndPassword()
        {
            var options = new UniqueDbConnectionProviderOptions("server", "database", "user", "password");
            var connectionProvider = new UniqueDbConnectionProvider(options);

            var connectionString = connectionProvider.GetSqlConnectionString();
            Console.WriteLine(connectionString);
        }
    }

    public class DatabaseLifecycleTests
    {
        [Scenario]
        public void ShouldDispose()
        {
            var options = new UniqueDbConnectionProviderOptions("ws2012sqlexp1\\sqlexpress", "database");
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

            var createDbText = string.Format("CREATE DATABASE [{0}];", connectionProvider.DbName);
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
