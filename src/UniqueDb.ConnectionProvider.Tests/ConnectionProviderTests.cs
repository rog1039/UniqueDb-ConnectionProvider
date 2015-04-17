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
            var connectionProvider = new UniqueDbConnectionCreator(options);

            var connectionString = connectionProvider.GetSqlConnectionString();
            Console.WriteLine(connectionString);
        }
    }
}
