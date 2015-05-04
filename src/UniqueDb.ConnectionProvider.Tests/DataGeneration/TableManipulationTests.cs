using FluentAssertions;
using UniqueDb.ConnectionProvider.Tests;
using UniqueDb.ConnectionProvider.Tests.DataGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class TableManipulationTests
    {
        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateTableShouldWork()
        {
            ISqlConnectionProvider sourceSqlConnectionProvider = null;
            UniqueDbConnectionProvider targetSqlConnectionProvider = null;
            SqlTableReference sourceSqlTableReference = null;
            SqlTableReference targetSqlTableReference = null;

            "Given a blank target database"
                ._(() =>
                {
                    targetSqlConnectionProvider =
                        new UniqueDbConnectionProvider(new UniqueDbConnectionProviderOptions(
                            "ws2012sqlexp1\\sqlexpress", "TableManipulationTests"));
                    targetSqlConnectionProvider.CreateDatabase();
                });

            using (targetSqlConnectionProvider.ToDisposable())
            {
                "Given a source database and a new blank database"
                ._(() =>
                {
                    sourceSqlConnectionProvider = LiveDbTestingSqlProvider.AdventureWorksDb;
                });

                "Given a source table to copy"
                    ._(() =>
                    {
                        sourceSqlTableReference = new SqlTableReference(
                            sourceSqlConnectionProvider, "Person.Person");
                    });

                "When copying the source table to the target db."
                    ._(() =>
                    {
                        targetSqlTableReference = new SqlTableReference(
                            targetSqlConnectionProvider, "dbo.Person");
                        TableManipulation.CopyTable(sourceSqlTableReference, targetSqlTableReference);
                    });

                "Then there should be a copy of the table at the target DB"
                    ._(() =>
                    {
                        var tableSchemas =
                            InformationSchemaMetadataExplorer.GetInformationSchemaTables(targetSqlConnectionProvider);
                        tableSchemas.Count.Should().Be(1);
                        tableSchemas[0].TABLE_NAME.Should().Be("Person");
                    });
            }
        }

        [Fact()]
        [Trait("Category", "Integration")]
        public void DropTableShouldWork()
        {
            ISqlConnectionProvider sourceSqlConnectionProvider = null;
            UniqueDbConnectionProvider targetSqlConnectionProvider = null;
            SqlTableReference sourceSqlTableReference = null;
            SqlTableReference targetSqlTableReference = null;

            "Given a table to drop in the database."
                ._(() =>
                {
                    targetSqlConnectionProvider =
                        new UniqueDbConnectionProvider(new UniqueDbConnectionProviderOptions(
                            "ws2012sqlexp1\\sqlexpress", "TableManipulationTests"));
                    targetSqlConnectionProvider.CreateDatabase();

                    sourceSqlConnectionProvider = LiveDbTestingSqlProvider.AdventureWorksDb;
                    sourceSqlTableReference = new SqlTableReference(
                        sourceSqlConnectionProvider, "Person.Person");
                    targetSqlTableReference = new SqlTableReference(
                        targetSqlConnectionProvider, "dbo.Person");
                    TableManipulation.CopyTable(sourceSqlTableReference, targetSqlTableReference);
                    var tableSchemas =
                        InformationSchemaMetadataExplorer.GetInformationSchemaTables(targetSqlConnectionProvider);
                    tableSchemas.Count.Should().Be(1);
                    tableSchemas[0].TABLE_NAME.Should().Be("Person");
                });

            using (targetSqlConnectionProvider.ToDisposable())
            {
                "When dropping the target table."
                    ._(() =>
                    {
                        TableManipulation.DropTable(targetSqlTableReference);
                    });

                "Then the table should be removed from the database."
                    ._(() =>
                    {
                        var tableSchemas =
                            InformationSchemaMetadataExplorer.GetInformationSchemaTables(targetSqlConnectionProvider);
                        tableSchemas.Count.Should().Be(0);
                    });
            }
        }
    }
}
