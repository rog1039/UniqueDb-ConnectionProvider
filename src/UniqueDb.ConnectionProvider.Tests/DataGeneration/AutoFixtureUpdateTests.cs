using AutoFixture;
using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.Crud;
using UniqueDb.ConnectionProvider.Tests.DataGeneration.AdventureWorks;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class AutoFixtureUpdateTests
{
    private bool PrintDebug = false;

    [Fact()]
    [Trait("Category", "Integration")]
    public void SimpleUpdateTest()
    {
        SqlLogger.LogSqlStatementAction = s =>
        {
            if (PrintDebug)
            {
                Console.WriteLine(s);
            }
        };

        Fixture        fixture       = new Fixture();
        AWBuildVersion buildVersion1 = fixture.Create<AWBuildVersion>();
        AWBuildVersion buildVersion2 = null;
        AWBuildVersion buildVersion3 = null;

        ISqlConnectionProvider db = SqlConnectionProviders.AdventureWorksDb;
            
        "Given a build version in the database"
            ._(() =>
            {
                db.Insert(buildVersion1, "AWBuildVersion", "dbo");
                buildVersion2 = db
                    .MyQuery<AWBuildVersion>($"[Database Version] = '{buildVersion1.Database_Version}'", "AWBuildVersion", "dbo")
                    .Single();

                PrintDebugInfo1(buildVersion2, buildVersion1);
            });

        "When updating a property on the build version"
            ._(() =>
            {
                buildVersion2.Database_Version = fixture.Create<string>().Substring(0, 25);
                db.Update(buildVersion2, x => new { x.SystemInformationID });
            });

        "Then the database record should be modified."
            ._(() =>
            {
                buildVersion3 = db
                    .MyQuery<AWBuildVersion>($"[SystemInformationID] = '{buildVersion2.SystemInformationID}'", "AWBuildVersion", "dbo")
                    .Single();
                buildVersion3.Should().NotBeNull();
                buildVersion3.Database_Version.Should().Be(buildVersion2.Database_Version);
                buildVersion3.Database_Version.Should().NotBe(buildVersion1.Database_Version);
            });
    }

    private void PrintDebugInfo1(AWBuildVersion buildVersionFromDb, AWBuildVersion buildVersion)
    {
        if (PrintDebug)
        {
            buildVersionFromDb.Should().NotBeNull();
            buildVersion.MakeList().PrintStringTable();
            buildVersionFromDb.MakeList().PrintStringTable();
        }
    }
}