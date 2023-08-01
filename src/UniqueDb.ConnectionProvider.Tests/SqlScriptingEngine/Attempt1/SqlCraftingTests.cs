using NUnit.Framework;
using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.CSharpGeneration.DesignTimeDataGeneration;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata;
using UniqueDb.ConnectionProvider.SqlMetadata.InformationSchema;
using UniqueDb.ConnectionProvider.SqlMetadata.SysTables;
using UniqueDb.ConnectionProvider.SqlScriptingEngine.Attempt1;
using Woeber.Logistics.FluentDbMigrations.Tests;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration.SqlManipulation.PiecewiseManipulation;

[TestFixture]
public class SqlCraftingTests
{
   [Test]
   public async Task GenerateTemporal()
   {
      var input      = new TemporalTableInput("dbo", "Main", "old", "Main_History");
      var enableSql  = EnableTemporalTable.GenerateSql(input);
      var disableSql = DisableTemporalTable.GenerateSql(input);

      enableSql.ToString().ToConsole();
      disableSql.ToString().ToConsole();
   }

   [Test]
   public async Task EndToEndAddColumnToTemporalTableAtIndex()
   {
      var result = await BuildAlterTableOutput();

      result.ToString().ToConsoleWriteLine();
   }

   [Test]
   public async Task EndToEndTestOfScript1()
   {
      var result = await BuildAlterTableOutput();
      var output = SqlOutputTextGenerator1.GetScript(result);
      output.ToConsole();
   }

   [Test]
   public async Task EndToEndTestOfScript2()
   {
      var result = await BuildAlterTableOutput();
      PrettyPrintSqlOutput(result);
   }

   private static void PrettyPrintSqlOutput(SqlOutputNode result)
   {
      var allSteps = Flattener.FlattenViaStack(result, result => result.Children);

      foreach (var step in allSteps)
      {
         var last = step.Last();
         var path = step.Select(x => x.GeneratorType).StringJoin(" |> ", (s, i) => $"[{i}] {s}");
         Console.WriteLine($"-- [{step.Count}] " + path);
         Console.WriteLine(last.SqlText.IndentWithTabs(step.Count * 1));
      }
   }

   private static async Task<SqlOutputNode> BuildAlterTableOutput()
   {
      var input = new AlterTable_AddColumnInput()
      {
         SqlTableReference = new SqlTableReference(SqlConnectionProviders.WideWorldImporters, "Warehouse.StockItems"),
         ColumnName        = "TankInfo",
         ColumnType        = new SqlDataType() { SqlType = "nvarchar(max)", Nullable = true },
         ColumnIndex       = 2
      };

      var fakeSchema = new FakeSchemaInformationProvider();
      fakeSchema._sisColumns = new List<SISColumn>()
      {
         new SISColumn() { COLUMN_NAME = "Col0", ORDINAL_POSITION = 1, DATA_TYPE = "int" },
         new SISColumn() { COLUMN_NAME = "Col1", ORDINAL_POSITION = 2, DATA_TYPE = "int" },
         new SISColumn() { COLUMN_NAME = "Col2", ORDINAL_POSITION = 3, DATA_TYPE = "int" },
      };
      fakeSchema._temporalList.Add("Warehouse.StockItems");
      fakeSchema._mainToHistoryTableLink = new MainToHistoryTableLink()
      {
         MainSchemaName    = "Warehouse",
         MainTableName     = "StockItems",
         HistorySchemaName = "Warehouse",
         HistoryTableName  = "StockItems_History",
      };

      var result = await AddColumnToTemporalAtIndex.GenerateSql(fakeSchema, input);
      return result;
   }

   [Test]
   public async Task TestAgainstActualDb()
   {
      var input = new AlterTable_AddColumnInput()
      {
         SqlTableReference = new(SqlConnectionProviders.WideWorldImporters, "Warehouse.StockItems"),
         ColumnName        = "MyNameColumn",
         ColumnType        = new SqlDataType("decimal(32,18)", true),
         ColumnIndex       = 9
      };
      var schemaProvider = new RealSchemaProvider();

      var result = await AddColumnToTemporalAtIndex.GenerateSql(schemaProvider, input);
      PrettyPrintSqlOutput(result);
   }
}

public class FakeSchemaInformationProvider : ISchemaInformationProvider
{
   public MainToHistoryTableLink       _mainToHistoryTableLink { get; set; }
   public List<SysForeignKey>  _fkConstraintColumns    { get; set; } = new();
   public List<SysIndexColumn>    _indexColumns           { get; set; } = new();
   public List<SISTableConstraint> _constraintInfos        { get; set; } = new();
   public List<SISColumn>              _sisColumns             { get; set; } = new();
   public SISTable                     _sisTable               { get; set; }
   public List<string>                 _temporalList           { get; set; } = new();

   public IList<SysForeignKey> GetForeignKeyDtos(SqlTableReference table)
   {
      return _fkConstraintColumns;
   }

   public IList<SysIndexColumn> GetIndices(SqlTableReference table)
   {
      return _indexColumns;
   }

   public IList<SISTableConstraint> GetConstraints(SqlTableReference table)
   {
      return _constraintInfos;
   }

   public IList<SISColumn> GetColumns(SqlTableReference table)
   {
      return _sisColumns;
   }

   public SISTable GetTable(SqlTableReference table)
   {
      return _sisTable;
   }

   public bool IsTemporal(SqlTableReference table)
   {
      return _temporalList.Contains(table.FullTableName());
   }

   public MainToHistoryTableLink TemporalTableInfo(SqlTableReference table)
   {
      return _mainToHistoryTableLink;
   }
}

public class RealSchemaProvider : ISchemaInformationProvider
{
   public IList<SysForeignKey> GetForeignKeyDtos(SqlTableReference table)
   {
      return SysSchemaExplorer.GetForeignKeyColumnDtos(table);
   }

   public IList<SysIndexColumn> GetIndices(SqlTableReference table)
   {
      return SysSchemaExplorer.GetIndexColumnDtos(table);
   }

   public IList<SISTableConstraint> GetConstraints(SqlTableReference table)
   {
      return InformationSchemaExplorer.GetSisTableConstraints(table);
   }

   public IList<SISColumn> GetColumns(SqlTableReference table)
   {
      return InformationSchemaExplorer.GetSisColumns(table);
   }

   public SISTable GetTable(SqlTableReference table)
   {
      return InformationSchemaExplorer.GetSisTable(table);
   }

   public bool IsTemporal(SqlTableReference table)
   {
      return SysSchemaExplorer.GetTemporalTableInfo(table) is not null;
   }

   public MainToHistoryTableLink TemporalTableInfo(SqlTableReference table)
   {
      return SysSchemaExplorer.GetTemporalTableInfo(table);
   }
}