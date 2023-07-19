using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;
using Woeber.Logistics.FluentDbMigrations.Tests;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration.SqlManipulation.PiecewiseManipulation;

public class SqlOutputNode
{
   public string               GeneratorType { get; set; }
   public string               SqlText       { get; set; }
   public IList<SqlOutputNode> Children      { get; set; }
}

public class AlterTable_AddColumnInput
{
   public SqlTableReference SqlTableReference { get; set; }
   public string            ColumnName        { get; set; }
   public SqlDataType       ColumnType        { get; set; }
   public int               ColumnIndex       { get; set; }
}

public class SqlDataType
{
   public string SqlType  { get; set; }
   public bool   Nullable { get; set; }

   public SqlDataType() { }

   public SqlDataType(string sqlType, bool nullable)
   {
      SqlType  = sqlType;
      Nullable = nullable;
   }

   public SqlDataType(SISColumn column)
   {
      Nullable = column.IS_NULLABLE == "true" ? true : false;
      SqlType  = column.GetSqlDataTypeString();
   }
}

public static class StringExtesions
{
   public static string AddLine(this string text, string input)
   {
      return text.IsNullOrWhitespace()
         ? input
         : text + "\r\n" + input;
   }
}

public interface ISchemaInformationProvider
{
   public IList<FkConstraintColumnDto>  GetForeignKeyDtos(SqlTableReference table);
   public IList<IndexColumnQueryDto>    GetIndices(SqlTableReference        table);
   public IList<TableConstraintInfoDto> GetConstraints(SqlTableReference    table);
   public IList<SISColumn>              GetColumns(SqlTableReference        table);
   public SISTable                      GetTable(SqlTableReference          table);
   public Task<bool>                    IsTemporal(SqlTableReference        table);
   public MainToHistoryTableLink        TemporalTableInfo(SqlTableReference table);
}

public class AddColumnToTemporalAtIndex
{
   public static async Task<string> GenerateSql(ISchemaInformationProvider schemaInformationProvider,
                                                AlterTable_AddColumnInput  input)
   {
      /*
       * Disable temporal table
       * Add column to main table at index
       * Add column to temporal table at index
       * Re-establish temporal table
       *
       * $table = dbo.[Production.JobActivity]
       * $tableHistory = dbo.[Production.JobActivity_History]
       * BeginTransaction
       * DisableTemporalTable($table)
       * AddColumnAtIndex($table, TankInfo nvarchar(max) NULL, index:3)
       * AddColumnAtIndex($tableHistory, TankInfo nvarchar(max) NULL, index:3)
       * EnableTemporalTable($table, $tableHistory)
       */

      // var temporalInput = new TemporalTableInput()

      string sql = String.Empty;

      sql = sql.AddLine(BeginTransaction.GenerateSql());

      var temporalInfo = schemaInformationProvider.TemporalTableInfo(input.SqlTableReference);
      TemporalTableInput? temporalTableInput = temporalInfo is not null
         ? new TemporalTableInput(
            temporalInfo.MainSchemaName,
            temporalInfo.MainTableName,
            temporalInfo.HistorySchemaName,
            temporalInfo.HistoryTableName)
         : null;
      if (temporalTableInput is not null)
      {
         sql = sql.AddLine(DisableTemporalTable.GenerateSql(temporalTableInput));
      }

      sql = sql.AddLine(AlterTable_AddColumn.GenerateSql(schemaInformationProvider, input)); //main table

      // sql.AddLine(AlterTable_AddColumn.GenerateSql(null)); //history table
      if (temporalTableInput is not null)
      {
         sql = sql.AddLine(EnableTemporalTable.GenerateSql(temporalTableInput));
      }

      sql = sql.AddLine(CommitTransaction.GenerateSql());
      return sql;
   }
}

public class CommitTransaction
{
   public static string GenerateSql()
   {
      return "--CommitTransaction\r\nCOMMIT TRANSACTION";
   }
}

public class BeginTransaction
{
   public static string GenerateSql()
   {
      return "--BeginTransaction\r\nBEGIN TRANSACTION";
   }
}

public record TemporalTableInput(string MainSchema,
                                 string MainTable,
                                 string HistorySchema,
                                 string HistoryTable);

public class EnableTemporalTable
{
   public static string GenerateSql(TemporalTableInput input)
   {
      return """
             ---EnableTemporalTable
             ALTER TABLE [$mainSchema].[$mainTable] SET
             (
               SYSTEM_VERSIONING = ON
               (HISTORY_TABLE = [$historySchema].[$historyTable])
             );
             """
            .MyReplace2("mainSchema",    input.MainSchema)
            .MyReplace2("mainTable",     input.MainTable)
            .MyReplace2("historySchema", input.HistorySchema)
            .MyReplace2("historyTable",  input.HistoryTable)
         ;
   }
}

public class DisableTemporalTable
{
   public static string GenerateSql(TemporalTableInput input)
   {
      return """
             ---DisableTemporalTable
             ALTER TABLE [$schema].[$table] SET (SYSTEM_VERSIONING = OFF);
             """
            .MyReplace2("schema", input.MainSchema)
            .MyReplace2("table",  input.MainTable)
         ;
   }
}

public class AlterTable_AddColumn
{
   public static string GenerateSql(ISchemaInformationProvider schemaInformationProvider,
                                    AlterTable_AddColumnInput  input)
   {
      /*
       * Steps:
       *    * Create Temp Table
       *       - if column is not null, need to create default constraint
       *    * Insert into from Source -> Temp table
       *    * Drop Source table
       *    * Rename Temp -> Source table
       *    * Add PK/FK/Indices to table
       */
      var sql = "---AlterTable";

      if (!input.ColumnType.Nullable)
         throw new NotImplementedException("No support for NOT NULL columns since we would then need to add" +
                                           "support for default constraints");

      /*
       * Create Temp Table
       */
      var sisColumns     = schemaInformationProvider.GetColumns(input.SqlTableReference);
      var allColumnInfos = sisColumns.Select(x => new ColumnInfo(x)).ToList();
      var newColumnInfo = new ColumnInfo()
      {
         ColumnName = input.ColumnName,
         ColumnType = new SqlDataType(input.ColumnType.SqlType, input.ColumnType.Nullable)
      };
      allColumnInfos.Insert(input.ColumnIndex - 1, newColumnInfo);

      var sourceTableName = new TableName(input.SqlTableReference);
      var tempTableName = new TableName(input.SqlTableReference.SchemaName, $"tmp_{input.SqlTableReference.TableName}");
      var createTempTableInput = new CreateTableInput()
      {
         TableName   = $"tmp_{input.SqlTableReference.TableName}",
         SchemaName  = input.SqlTableReference.SchemaName,
         ColumnInfos = allColumnInfos
      };

      var createTempTableSql = CreateTable.GenerateSql(schemaInformationProvider, createTempTableInput);

      /*
       * Insert into Temp Table
       */
      var insertIntoInput = new InsertIntoInput()
      {
         SourceTable = sourceTableName,
         TargetTable = tempTableName,
         ColumnNames = sisColumns.Select(x => x.COLUMN_NAME).ToList(),
      };
      var insertIntoSql = InsertInto.GenerateSql(insertIntoInput);

      /*
       * Drop Source Table
       */
      var dropTableSql = DropTable.GenerateSql(sourceTableName);


      /*
       * Rename Temp -> Source Table
       */
      var renameTableSql = RenameTable.GenerateSql(tempTableName, sourceTableName);


      sql = sql.AddLine("-- --Create Temp Table:");
      sql = sql.AddLine(createTempTableSql);
      sql = sql.AddLine("-- --Insert Into SQL:");
      sql = sql.AddLine(insertIntoSql);
      sql = sql.AddLine("-- --Drop Table SQL:");
      sql = sql.AddLine(dropTableSql);
      sql = sql.AddLine("-- --Rename Temp to Source Table SQL:");
      sql = sql.AddLine(renameTableSql);

      return sql;
   }
}

public class RenameTable
{
   public static string GenerateSql(TableName sourceTable, TableName targetTable)
   {
      return $"EXEC sp_rename '{sourceTable}', '{targetTable.Table}';";
   }
}

public class DropTable
{
   public static string GenerateSql(TableName tableName) => $"DROP TABLE {tableName}";
}

public class CreateTableInput
{
   public string           SchemaName { get; set; }
   public string           TableName  { get; set; }
   public List<ColumnInfo> ColumnInfos = new();
}

public class ColumnInfo
{
   public string      ColumnName { get; set; }
   public SqlDataType ColumnType { get; set; }

   public ColumnInfo() { }

   public ColumnInfo(SISColumn column)
   {
      ColumnName = column.COLUMN_NAME;
      ColumnType = new SqlDataType(column);
   }

   public string ToString()
   {
      return $"{ColumnName} {ColumnType.SqlType} {NullabilityToString(ColumnType.Nullable)}";
   }

   private string NullabilityToString(bool isNullable)
   {
      return isNullable ? "NULL" : "NOT NULL";
   }

   public static string NullabilityToString(Nullability nullability)
   {
      return nullability switch
      {
         Nullability.Null    => "NULL",
         Nullability.NotNull => "NOT NULL",
         _                   => throw new ArgumentOutOfRangeException(nameof(nullability), nullability, null)
      };
   }
}

public class CreateTable
{
   public static string GenerateSql(ISchemaInformationProvider schemaInformationProvider, CreateTableInput input)
   {
      string sql = "--Create Table";


      var tableName = new TableName(input.SchemaName, input.TableName);
      var columnText = input.ColumnInfos
         .Select(x => x.ToString())
         .StringJoin(",\r\n");

      sql = sql.AddLine($"CREATE TABLE {tableName}");
      sql = sql.AddLine($"(");
      sql = sql.AddLine(columnText);
      sql = sql.AddLine($")");

      return sql;
   }
}

public class InsertIntoInput
{
   public TableName    SourceTable { get; set; }
   public TableName    TargetTable { get; set; }
   public List<string> ColumnNames { get; set; }
}

public class InsertInto
{
   public static string GenerateSql(InsertIntoInput input)
   {
      string sql = String.Empty;

      var columnList = input.ColumnNames.StringJoinCommaNewLine();

      sql = sql.AddLine($"SET IDENTITY_INSERT {input.TargetTable} ON");
      sql = sql.AddLine($"INSERT INTO {input.TargetTable} WITH (TABLOCK) (");
      sql = sql.AddLine(columnList);
      sql = sql.AddLine(")");
      sql = sql.AddLine("SELECT ");
      sql = sql.AddLine(columnList);
      sql = sql.AddLine($"FROM {input.SourceTable}");
      sql = sql.AddLine($"SET IDENTITY_INSERT {input.TargetTable} OFF");

      return sql;
   }
}