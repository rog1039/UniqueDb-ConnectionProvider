﻿using FluentAssertions;
using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata;
using UniqueDb.ConnectionProvider.SqlScripting;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class TableManipulationTests
{
    [Fact()]
    [Trait("Category", "Integration")]
    public void CreateTableShouldWork()
    {
        ISqlConnectionProvider     sourceSqlConnectionProvider = null;
        UniqueDbConnectionProvider targetSqlConnectionProvider = null;
        SqlTableReference          sourceSqlTableReference     = null;
        SqlTableReference          targetSqlTableReference     = null;

        "Given a blank target database"
            ._(() =>
            {
                targetSqlConnectionProvider =
                    new UniqueDbConnectionProvider(new UniqueDbConnectionProviderOptions(
                                                       "ws2012sqlexp1\\sqlexpress", "TableManipulationTests"));
                targetSqlConnectionProvider.CreateDatabase();
            });

        using (targetSqlConnectionProvider.ToSelfDeletingDisposable())
        {
            "Given a source database and a new blank database"
                ._(() =>
                {
                    sourceSqlConnectionProvider = SqlConnectionProviders.AdventureWorksDb;
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
                    TableManipulation.CopyTableStructure(sourceSqlTableReference, targetSqlTableReference);
                });

            "Then there should be a copy of the table at the target DB"
                ._(() =>
                {
                    var tableSchemas =
                        InformationSchemaExplorer.GetSisTablesOnly(targetSqlConnectionProvider);
                    tableSchemas.Count.Should().Be(1);
                    tableSchemas[0].TABLE_NAME.Should().Be("Person");
                });
        }
    }

    [Fact()]
    [Trait("Category", "Integration")]
    public void DropTableShouldWork()
    {
        ISqlConnectionProvider     sourceSqlConnectionProvider = null;
        UniqueDbConnectionProvider targetSqlConnectionProvider = null;
        SqlTableReference          sourceSqlTableReference     = null;
        SqlTableReference          targetSqlTableReference     = null;

        "Given a table to drop in the database."
            ._(() =>
            {
                targetSqlConnectionProvider =
                    new UniqueDbConnectionProvider(new UniqueDbConnectionProviderOptions(
                                                       "ws2012sqlexp1\\sqlexpress", "TableManipulationTests"));
                targetSqlConnectionProvider.CreateDatabase();

                sourceSqlConnectionProvider = SqlConnectionProviders.AdventureWorksDb;
                sourceSqlTableReference = new SqlTableReference(
                    sourceSqlConnectionProvider, "Person.Person");
                targetSqlTableReference = new SqlTableReference(
                    targetSqlConnectionProvider, "dbo.Person");
                TableManipulation.CopyTableStructure(sourceSqlTableReference, targetSqlTableReference);
                var tableSchemas =
                    InformationSchemaExplorer.GetSisTablesOnly(targetSqlConnectionProvider);
                tableSchemas.Count.Should().Be(1);
                tableSchemas[0].TABLE_NAME.Should().Be("Person");
            });

        using (targetSqlConnectionProvider.ToSelfDeletingDisposable())
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
                        InformationSchemaExplorer.GetSisTablesOnly(targetSqlConnectionProvider);
                    tableSchemas.Count.Should().Be(0);
                });
        }
    }
}