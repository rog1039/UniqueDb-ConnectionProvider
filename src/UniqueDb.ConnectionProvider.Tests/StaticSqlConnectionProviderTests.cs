using System;
using FluentAssertions;
using Xbehave;

namespace UniqueDb.ConnectionProvider.Tests
{
    public class StaticSqlConnectionProviderTests
    {
        private string serverName = "sqlserver";
        private string database = "db1";
        private string username = "admin";
        private string password = "1234";

        [Scenario]
        public void WithoutIntegratedSecurityTest(StaticSqlConnectionProvider staticSqlConnectionProvider)
        {
            "Given a static sql connection provider with a username and password"
                ._(() => staticSqlConnectionProvider = new StaticSqlConnectionProvider(serverName,database,username,password));

            "Then the connection string should contain the username and password"
                ._(() =>
                {
                    var connectionString = staticSqlConnectionProvider.GetSqlConnectionString();
                    Console.WriteLine(connectionString);
                    connectionString.Contains(serverName).Should().BeTrue();
                    connectionString.Contains(database).Should().BeTrue();
                    connectionString.Contains(username).Should().BeTrue();
                    connectionString.Contains(password).Should().BeTrue();
                });
        }

        [Scenario]
        public void WithIntegratedSecurityTest(StaticSqlConnectionProvider staticSqlConnectionProvider)
        {
            "Given a static sql connection provider with integrated security"
                ._(() => staticSqlConnectionProvider = new StaticSqlConnectionProvider(serverName, database));

            "Then the connection string should not contain the username and password"
                ._(() =>
                {
                    var connectionString = staticSqlConnectionProvider.GetSqlConnectionString();
                    Console.WriteLine(connectionString);
                    connectionString.Contains(serverName).Should().BeTrue();
                    connectionString.Contains(database).Should().BeTrue();
                    connectionString.Contains(username).Should().BeFalse();
                    connectionString.Contains(password).Should().BeFalse();
                });
        }
    }
}