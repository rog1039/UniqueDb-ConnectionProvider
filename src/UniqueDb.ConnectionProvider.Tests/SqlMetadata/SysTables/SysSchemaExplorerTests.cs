using NUnit.Framework;
using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

[TestFixture]
public class InformationSchemaExplorerTests
{
    private readonly SqlTableReference _sqlTableForStockItems = new(SqlConnectionProviders.WideWorldImporters,
                                                                    "Warehouse.StockItems");

    [Test]
    public void GetInformationSchemaTablesForAllTables()
    {
        var schemaTables = InformationSchemaExplorer
            .GetSisTablesOnly(SqlConnectionProviders.AdventureWorksDb)
            .OrderBy(x => x.TABLE_SCHEMA)
            .ThenBy(x => x.TABLE_NAME);
        schemaTables.PrintStringTable();
    }

    [Test]
    public async Task GetConstraints()
    {
        var constraints = InformationSchemaExplorer.GetSisTableConstraints(_sqlTableForStockItems);
        constraints.PrintStringTable();
    }

}

[TestFixture]
public class SysSchemaExplorerTests
{
    private readonly SqlTableReference _sqlTableForStockItems = new(SqlConnectionProviders.WideWorldImporters,
                                                                    "Warehouse.StockItems");
                                                                    
    [Test]
    public async Task GetComputedColumns()
    {
        var computedColumns = SysSchemaExplorer.GetComputedColumns(_sqlTableForStockItems);
        computedColumns.PrintStringTable();
    }

    [Test]
    public async Task GetForeignKeys()
    {
        var foreignKeyColumnDtos = SysSchemaExplorer.GetForeignKeyColumnDtos(_sqlTableForStockItems);
        foreignKeyColumnDtos.PrintStringTable();
    }

    [Test]
    public async Task GetIndices()
    {
        var indices = SysSchemaExplorer.GetIndexColumnDtos(_sqlTableForStockItems);
        indices.PrintStringTable();
    }
    
    [Test]
    public async Task GetTemporalTableInfo()
    {
        var indices = SysSchemaExplorer.GetTemporalTableInfo(_sqlTableForStockItems);
        indices.MakeList().PrintStringTable();
    }
    
    [Test]
    public async Task GetAllTables()
    {
        var tables = SysSchemaExplorer.GetAllTables(SqlConnectionProviders.WideWorldImporters);
        tables.PrintStringTable();
    }

    [Test]
    public async Task GetAllColumns()
    {
        var tables = SysSchemaExplorer.GetAllColumns(SqlConnectionProviders.WideWorldImporters);
        tables.PrintStringTable();
    }

    [Test]
    public async Task GetTable()
    {
        var tables = SysSchemaExplorer.GetTable(_sqlTableForStockItems);
        tables.PrintStringTable();
    }

    [Test]
    public async Task GetColumns()
    {
        var tables = SysSchemaExplorer.GetColumns(_sqlTableForStockItems);
        tables.PrintStringTable();
    }
}