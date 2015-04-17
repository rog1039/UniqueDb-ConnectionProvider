using System;
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
            var options = new UniqueDbConnectionProviderOptions("server", "database");
            var connectionProvider = new UniqueDbConnectionProvider(options);

            using (var lifecycle = connectionProvider.ToDispopsable())
            {
                
            }
        }
    }
}
