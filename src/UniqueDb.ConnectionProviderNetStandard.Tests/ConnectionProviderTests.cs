using System;
using Xbehave;

namespace UniqueDb.ConnectionProvider.Tests
{
    public class UniqueDbConnectionProviderTests
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
}
