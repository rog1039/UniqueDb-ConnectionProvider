using FluentAssertions;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using Xbehave;

namespace UniqueDb.ConnectionProvider.Tests;

public class OldDatabaseDeleterTests
{
    [Scenario]
    public void TestGetDatabaseNamesOlderThan()
    {
        var format = "yyMMdd.HHmmss.fff";
            
        var databaseNameList = new List<string>()
        {
            "aslkdjfklasjdlk",
            "asdfas-(210938091283)-alksjdfa",
            MakeName(DateTime.Now.AddDays(-1),       format),
            MakeName(DateTime.Now.AddHours(-2),      format),
            MakeName(DateTime.Now.AddHours(-1.0001), format),
            MakeName(DateTime.Now.AddHours(-1),      format),
            MakeName(DateTime.Now.AddHours(-0.9999), format),
            MakeName(DateTime.Now.AddHours(-.5),     format),
            MakeName(DateTime.Now.AddDays(0),        format),
        };

        var databasesLessThan1HourOld = OldDatabaseDeleter.SelectDatabaseNamesOlderThan(databaseNameList, format,
            TimeSpan.FromHours(1));

        databasesLessThan1HourOld.Count().Should().Be(4);
    }

    private string MakeName(DateTime dateTime, string format)
    {
        var dateTimeAsString = dateTime.ToString(format);
        return String.Format("a-({0})-alksdfjasd", dateTimeAsString);
    }

    [Scenario]
    public void IntegrationTest()
    {
        var databasePrefix = "OldDatabaseDeleterTests.IntegrationTest-" +
                             Guid.NewGuid().ToString("N").Substring(0, 8);
        UniqueDbConnectionProvider uniqueDbProvider1 = null;
        UniqueDbConnectionProvider uniqueDbProvider2 = null;

        "Given two databases created 7 seconds apart"
            ._(() =>
            {
                uniqueDbProvider1 = new UniqueDbConnectionProvider(new UniqueDbConnectionProviderOptions(
                                                                       TestingConstants.SqlServerInstance, databasePrefix));
                uniqueDbProvider1.CreateDatabase();

                Thread.Sleep(7000);

                uniqueDbProvider2 = new UniqueDbConnectionProvider(new UniqueDbConnectionProviderOptions(
                                                                       TestingConstants.SqlServerInstance, databasePrefix));
                uniqueDbProvider2.CreateDatabase();

                OldDatabaseDeleter.GetOldDatabasesFromUniqueDb(uniqueDbProvider1, TimeSpan.FromSeconds(0))
                    .Count.Should()
                    .Be(2);
            });


        "When we delete databases older than 7 seconds, we should delete the first of the two"
            ._(() =>
            {
                OldDatabaseDeleter.DeleteOldDatabases(uniqueDbProvider1, TimeSpan.FromSeconds(7));
                OldDatabaseDeleter.GetOldDatabasesFromUniqueDb(uniqueDbProvider2, TimeSpan.FromSeconds(0))
                    .Count.Should()
                    .Be(1);
            });

        "Now let's delete all the datbases to clean up"
            ._(() =>
            {
                uniqueDbProvider2.AndAutoDeleteDbOlderThan(TimeSpan.FromSeconds(0));
                OldDatabaseDeleter.GetOldDatabasesFromUniqueDb(uniqueDbProvider1, TimeSpan.FromSeconds(0))
                    .Count.Should()
                    .Be(0);
            });
    }
}