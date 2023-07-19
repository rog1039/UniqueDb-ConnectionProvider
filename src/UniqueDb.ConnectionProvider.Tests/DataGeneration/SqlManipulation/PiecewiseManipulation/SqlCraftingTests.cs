using NUnit.Framework;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.DesignTimeDataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;
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

      enableSql.ToConsole();
      disableSql.ToConsole();
   }

   [Test]
   public async Task EndToEndAddColumnToTemporalTableAtIndex()
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

      result.ToConsoleWriteLine();
   }
}

public class FakeSchemaInformationProvider : ISchemaInformationProvider
{
   public MainToHistoryTableLink       _mainToHistoryTableLink { get; set; }
   public List<FkConstraintColumnDto>  _fkConstraintColumns    { get; set; } = new();
   public List<IndexColumnQueryDto>    _indexColumns           { get; set; } = new();
   public List<TableConstraintInfoDto> _constraintInfos        { get; set; } = new();
   public List<SISColumn>              _sisColumns             { get; set; } = new();
   public SISTable                     _sisTable               { get; set; }
   public List<string>                 _temporalList           { get; set; } = new();

   public IList<FkConstraintColumnDto> GetForeignKeyDtos(SqlTableReference table)
   {
      return _fkConstraintColumns;
   }

   public IList<IndexColumnQueryDto> GetIndices(SqlTableReference table)
   {
      return _indexColumns;
   }

   public IList<TableConstraintInfoDto> GetConstraints(SqlTableReference table)
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

   public Task<bool> IsTemporal(SqlTableReference table)
   {
      return Task.FromResult(_temporalList.Contains(table.FullTableName()));
   }

   public MainToHistoryTableLink TemporalTableInfo(SqlTableReference table)
   {
      return _mainToHistoryTableLink;
   }
}