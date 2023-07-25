using NUnit.Framework;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class InformationSchemaMetadataExplorerTests
{
    [Fact()]
    [Trait("Category", "Integration")]
    public void GetInformationSchemaTablesForAllTables()
    {
        var schemaTables = InformationSchemaMetadataExplorer
            .GetInformationSchemaTablesOnly(SqlConnectionProviders.AdventureWorksDb)
            .OrderBy(x => x.TABLE_SCHEMA)
            .ThenBy(x => x.TABLE_NAME);
        Console.WriteLine(schemaTables.ToStringTable());
    }

    [Test]
    public async Task GetComputedColumns()
    {
        var computedColumns = InformationSchemaMetadataExplorer
            .GetComputedColumns(new SqlTableReference(SqlConnectionProviders.WideWorldImporters,
                                                      "Warehouse.StockItems"));
        computedColumns.PrintStringTable();
    }

    [Test]
    [Fact]
    public async Task GetForeignKeys()
    {
        var foreignKeyColumnDtos = InformationSchemaMetadataExplorer
            .GetForeignKeyColumnDtos(new SqlTableReference(SqlConnectionProviders.WideWorldImporters,
                                                      "Warehouse.StockItems"));
        foreignKeyColumnDtos.PrintStringTable();
    }

    [Test]
    [Fact]
    public async Task GetConstraints()
    {
        var constraints = InformationSchemaMetadataExplorer
            .GetTableConstraints(new SqlTableReference(SqlConnectionProviders.WideWorldImporters,
                                    "Warehouse.StockItems"));
        constraints.PrintStringTable();
    }

    [Test]
    [Fact]
    public async Task GetIndices()
    {
        var indices = InformationSchemaMetadataExplorer
            .GetIndexColumnDtos(new SqlTableReference(SqlConnectionProviders.WideWorldImporters,
                                    "Warehouse.StockItems"));
        indices.PrintStringTable();
    }
}