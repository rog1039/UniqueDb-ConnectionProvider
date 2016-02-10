using System;
using System.Runtime.InteropServices.ComTypes;
using FluentAssertions;
using Xbehave;
using Xunit;

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

    public class BaseSqlConnectionProvider_ServerAndInstanceName_Tests
    {
        [Theory]
        [Trait("Category", "Instant")]
        [InlineData("server\\instance", "server", "instance")]
        [InlineData("server", "server", "")]
        [InlineData("server\\", "server", "")]
        public void TestVariousServerInstanceNames(string server, string serverName, string instanceName)
        {
            var scp = GetConnectionProvider(server);
            scp.JustServerName.Should().Be(serverName);
            scp.JustInstanceName.Should().Be(instanceName);
        }

        private ISqlConnectionProvider GetConnectionProvider(string server)
        {
            return new StaticSqlConnectionProvider(server, "");

        }
    }

    public class ServerInstanceNameTests
    {
        [Theory]
        [Trait("Category", "Instant")]
        [InlineData("server\\instance", "server", "instance")]
        [InlineData("server", "server", "")]
        [InlineData("server\\", "server", "")]
        public void ParseTests(string server, string serverName, string instanceName)
        {
            var serverInstanceName = ServerInstanceName.Parse(server);
            serverInstanceName.ServerName.Should().Be(serverName);
            serverInstanceName.InstanceName.Should().Be(instanceName);
        }
    }
}