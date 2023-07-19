using System.Text;
using NUnit.Framework;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;
using Woeber.Logistics.FluentDbMigrations.Tests;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration.SqlManipulation.PiecewiseManipulation;

public class SqlOutputNode
{
   public string              GeneratorType { get; set; }
   public List<SqlOutputNode> Children      { get; set; } = new();
   public string              SqlText       => _stringBuilder.ToString();

   private StringBuilder _stringBuilder = new();

   public SqlOutputNode(string creator, string text = null)
   {
      GeneratorType = creator;

      if (!string.IsNullOrWhiteSpace(text)) _stringBuilder.AppendLine(text);
   }

   public SqlOutputNode AddLine(string line)
   {
      _stringBuilder.AppendLine(line);
      return this;
   }

   public void AddChild(SqlOutputNode child) => Children.Add(child);

   public string GetScript1()
   {
      string output = String.Empty;
      output.AddLine($"-- GenType: {GeneratorType}");
      output.AddLine(_stringBuilder.ToString());
      foreach (var child in Children)
      {
         output.AddLine(child.GetScript1());
      }

      return output;
   }

   public override string ToString()
   {
      var output = new StringBuilder();
      output.Append(_stringBuilder);
      foreach (var child in Children)
      {
         output.AppendLine(child.ToString());
      }

      return output.ToString();
   }
}

public class SqlOutputTextGenerator1
{
   public static string GetScript(SqlOutputNode node)
   {
      var stack = new AncestorStack<SqlOutputNode>();
      stack.Push(node);

      var sb    = new StringBuilder();
      var level = -1;

      while (stack.Count > 0)
      {
         var n = stack.Peek();

         sb.AppendLine($"-- ({stack.Count})" + GetPath(stack));
         sb.AppendLine(n.SqlText);
         stack.Pop();

         foreach (var child in ((IEnumerable<SqlOutputNode>) n.Children).Reverse())
         {
            stack.Push(child);
         }
      }

      return sb.ToString();
   }

   public static string GetPath(AncestorStack<SqlOutputNode> stack)
   {
      var text = stack.Select(x => x.GeneratorType).StringJoin(".");
      return text;
   }
}

public class SqlOutputTextGenerator2
{
   public static string GetScript2(SqlOutputNode node)
   {
      var ancestorStack = new AncestorStack<SqlOutputNode>();
      ancestorStack.Push(node);
      var sb = new StringBuilder();
      while (true)
      {
         var n = ancestorStack.Peek();
         sb.AppendLine($"-- {SqlOutputTextGenerator1.GetPath(ancestorStack)}");
         sb.AppendLine(n.SqlText);

         throw new NotImplementedException();
      }

      throw new NotImplementedException();
   }
}

public static class TreeFlattener
{
   public static List<List<T>> Flatten<T>(T item, Func<T,List<T>> childFunc)
   {
      var output = new List<List<T>>();
      var stack  = new Stack<T>();
      stack.Push(item);
      
      while (true)
      {
         var path = new List<T>();
         path.AddRange(stack);
         throw new NotImplementedException();
      }
   }
   
   public static List<List<T>> Flatten<T>(IList<T> item, Func<T,List<T>> childFunc)
   {
      var output = new List<List<T>>();
      var stack  = new Stack<T>();
      
      while (true)
      {
         output.Add(new List<T>(item));
      }
   }
}

public class AncestorStack<T> : List<T>
{
   public void Push(T obj)
   {
      Add(obj);
   }

   public T Pop()
   {
      var obj = this[Count - 1];
      RemoveAt(Count - 1);
      return obj;
   }

   public T Peek()
   {
      return this[Count - 1];
   }
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
   public static async Task<SqlOutputNode> GenerateSql(ISchemaInformationProvider schemaInformationProvider,
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

      var output = new SqlOutputNode(nameof(AddColumnToTemporalAtIndex));

      output.AddChild(BeginTransaction.GenerateSql());

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
         output.AddChild(DisableTemporalTable.GenerateSql(temporalTableInput));
      }

      output.AddChild(AlterTable_AddColumn.GenerateSql(schemaInformationProvider, input)); //main table

      // sql.AddLine(AlterTable_AddColumn.GenerateSql(null)); //history table
      if (temporalTableInput is not null)
      {
         output.AddChild(EnableTemporalTable.GenerateSql(temporalTableInput));
      }

      output.AddChild(CommitTransaction.GenerateSql());
      return output;
   }
}

public class CommitTransaction
{
   public static SqlOutputNode GenerateSql()
   {
      return new SqlOutputNode(nameof(CommitTransaction), "COMMIT TRANSACTION");
   }
}

public class BeginTransaction
{
   public static SqlOutputNode GenerateSql()
   {
      return new SqlOutputNode(nameof(BeginTransaction), "BEGIN TRANSACTION");
   }
}

public record TemporalTableInput(string MainSchema,
                                 string MainTable,
                                 string HistorySchema,
                                 string HistoryTable);

public class EnableTemporalTable
{
   public static SqlOutputNode GenerateSql(TemporalTableInput input)
   {
      return new(nameof(EnableTemporalTable),
                 """
                    ALTER TABLE [$mainSchema].[$mainTable] SET
                    (
                      SYSTEM_VERSIONING = ON
                      (HISTORY_TABLE = [$historySchema].[$historyTable])
                    );
                    """
                    .MyReplace2("mainSchema",    input.MainSchema)
                    .MyReplace2("mainTable",     input.MainTable)
                    .MyReplace2("historySchema", input.HistorySchema)
                    .MyReplace2("historyTable",  input.HistoryTable))
         ;
   }
}

public class DisableTemporalTable
{
   public static SqlOutputNode GenerateSql(TemporalTableInput input)
   {
      return new(nameof(DisableTemporalTable),
                 """
                    ALTER TABLE [$schema].[$table] SET (SYSTEM_VERSIONING = OFF);
                    """
                    .MyReplace2("schema", input.MainSchema)
                    .MyReplace2("table",  input.MainTable));
   }
}

public class AlterTable_AddColumn
{
   public static SqlOutputNode GenerateSql(ISchemaInformationProvider schemaInformationProvider,
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


      var output = new SqlOutputNode(nameof(AlterTable_AddColumn));

      output.Children.Add(createTempTableSql);
      output.Children.Add(insertIntoSql);
      output.Children.Add(dropTableSql);
      output.Children.Add(renameTableSql);

      return output;
   }
}

public class RenameTable
{
   public static SqlOutputNode GenerateSql(TableName sourceTable, TableName targetTable)
   {
      return new SqlOutputNode(nameof(RenameTable), $"EXEC sp_rename '{sourceTable}', '{targetTable.Table}';");
   }
}

public class DropTable
{
   public static SqlOutputNode GenerateSql(TableName tableName) => new(nameof(DropTable), $"DROP TABLE {tableName}");
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
   public static SqlOutputNode GenerateSql(ISchemaInformationProvider schemaInformationProvider, CreateTableInput input)
   {
      var tableName = new TableName(input.SchemaName, input.TableName);
      var columnText = input.ColumnInfos
         .Select(x => x.ToString())
         .StringJoin(",\r\n");

      var output = new SqlOutputNode(nameof(CreateTable));

      output.AddLine($"CREATE TABLE {tableName}");
      output.AddLine($"(");
      output.AddLine(columnText);
      output.AddLine($")");

      return output;
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
   public static SqlOutputNode GenerateSql(InsertIntoInput input)
   {
      var output     = new SqlOutputNode(nameof(InsertInto));
      var columnList = input.ColumnNames.StringJoinCommaNewLine();

      output.AddLine($"SET IDENTITY_INSERT {input.TargetTable} ON");
      output.AddLine($"INSERT INTO {input.TargetTable} WITH (TABLOCK) (");
      output.AddLine(columnList);
      output.AddLine(")");
      output.AddLine("SELECT ");
      output.AddLine(columnList);
      output.AddLine($"FROM {input.SourceTable}");
      output.AddLine($"SET IDENTITY_INSERT {input.TargetTable} OFF");

      return output;
   }
}