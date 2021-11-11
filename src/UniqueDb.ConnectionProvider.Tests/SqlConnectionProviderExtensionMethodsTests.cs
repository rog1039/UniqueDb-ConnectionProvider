using AutoFixture;
using FluentAssertions;
using Xbehave;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests;

public class SqlConnectionProviderExtensionMethodsTests
{
    [Scenario]
    public void TestExecute()
    {
        var options            = new UniqueDbConnectionProviderOptions("ws2012sqlexp1\\sqlexpress", "autodisposedatabase");
        var connectionProvider = new UniqueDbConnectionProvider(options);
        using (connectionProvider.ToSelfDeletingDisposable())
        {
            connectionProvider.CreateDatabase();
            connectionProvider.ExecuteNonDapper("Use [" + connectionProvider.DatabaseName + "]");
        }
    }

    [Scenario]
    public void TestExecuteScalar()
    {
        var options            = new UniqueDbConnectionProviderOptions("ws2012sqlexp1\\sqlexpress", "autodisposedatabase");
        var connectionProvider = new UniqueDbConnectionProvider(options);
        using (connectionProvider.ToSelfDeletingDisposable())
        {
            connectionProvider.CreateDatabase();
            connectionProvider.ExecuteScalar<int>("Select 1");
        }
    }

    [Fact()]
    [Trait("Category", "Integration")]
    public void DoesDatabaseExist_Test()
    {
        var fixture            = new Fixture();
        var madeUpDatabaseName = fixture.Create<string>();
            
        var options            = new UniqueDbConnectionProviderOptions("ws2012sqlexp1\\sqlexpress", "autodisposedatabase");
        var connectionProvider = new UniqueDbConnectionProvider(options);

        var doesMadeUpDatabaseExist = connectionProvider.DoesDatabaseExist(madeUpDatabaseName);
        doesMadeUpDatabaseExist.Should().BeFalse();
            
        var doesConnectionProviderDatabaseExist = connectionProvider.DoesDatabaseExist();
        doesMadeUpDatabaseExist.Should().BeFalse();
            
        var doesActualDatabaseExist = connectionProvider.DoesDatabaseExist("JobMgmt");
        doesActualDatabaseExist.Should().BeTrue();
    }
}