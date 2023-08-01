using NUnit.Framework;
using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata.SysTables.VeryNiceCopies;
using UniqueDb.ConnectionProvider.Tests.DataGeneration;

namespace UniqueDb.ConnectionProvider.Tests.SqlScriptingEngine.Attempt2.VeryNiceCopiesTests;

[TestFixture]
public class VeryNiceBuilderTests
{
   private SqlTableReference _sqlTableRef;

   private SqlTableReference _orderLinesTable = new(SqlConnectionProviders.PbsiDatabase, "dbo.OrderLines");

   [SetUp]
   public void Setup()
   {
      _sqlTableRef = new SqlTableReference(SqlConnectionProviders.WideWorldImporters, "Warehouse.StockItems");
   }

   [Test]
   public async Task GetTableSpec()
   {
      var result = await VeryNiceBuilder.BuildTableSpec(_sqlTableRef);
      result.MakeList().PrintStringTable();
      result.ColumnSpecs.PrintStringTable();
   }

   [Test]
   public async Task GetColumns()
   {
      var result = await VeryNiceBuilder.GetColumnSpecs(_sqlTableRef);
      result.PrintStringTable();
   }

   [Test]
   public async Task GetIndexes()
   {
      var result = await _sqlTableRef.GetSysIndexesNicer();
      result.PrintStringTable();

      (await _orderLinesTable.GetSysIndexesNicer()).PrintStringTable();
      var indexSpecs = (await VeryNiceBuilder.GetIndexSpec(_orderLinesTable));
      indexSpecs.PrintStringTable();
      
      foreach (var indexSpec in indexSpecs)
      {
         Console.WriteLine(indexSpec.IndexName);
         indexSpec.IndexColumnSpecs.PrintStringTable();
      }
   }

   [Test]
   public async Task GetIndexColumns()
   {
      var result = await _sqlTableRef.GetSysIndexColumnsNicer();
      result.PrintStringTable();

      var indexColumns = await _orderLinesTable.GetSysIndexColumnsNicer();
      indexColumns.PrintStringTable();

      var indexColumnSpecs = await _orderLinesTable.GetIndexColumnSpecs();
      indexColumnSpecs.PrintStringTable();
   }
}