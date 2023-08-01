using NUnit.Framework;
using UniqueDb.ConnectionProvider;
using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata;
using UniqueDb.ConnectionProvider.SqlScriptingEngine.Attempt1;

namespace Woeber.Logistics.FluentDbMigrations.Tests;

public class TestSqlTables
{
   public static ISqlConnectionProvider Logistics = new StaticSqlConnectionProvider("WS2016Sql", "WoeberLogisticsDb");

   public static SqlTableReference Order_Head  = new SqlTableReference(Logistics, "PbsiWM", "Order_Head");
   public static SqlTableReference JobActivity = new SqlTableReference(Logistics, "dbo",    "Production.JobActivity");
}

[TestFixture]
public class SqlCrafting
{
   [Test]
   public async Task SchemaExploreIndex1()
   {
      var index1 = SysSchemaExplorer.GetIndexColumnDtos(TestSqlTables.Order_Head);
      index1.PrintStringTable();
      SysSchemaExplorer.GetIndexColumnDtos(TestSqlTables.JobActivity).PrintStringTable();
   }

   [Test]
   public async Task SchemaExploreConstraints1()
   {
      var index1 = InformationSchemaExplorer.GetSisTableConstraints(TestSqlTables.Order_Head);
      index1.PrintStringTable();
      InformationSchemaExplorer.GetSisTableConstraints(TestSqlTables.JobActivity).PrintStringTable();
   }

   [Test]
   public async Task SchemaExploreForeignKeys()
   {
      var index1 = SysSchemaExplorer.GetForeignKeyColumnDtos(TestSqlTables.JobActivity);
      index1.PrintStringTable();
      InformationSchemaExplorer.GetSisTableConstraints(TestSqlTables.JobActivity).PrintStringTable();
   }

   [Test]
   public async Task SchemaExploration()
   {
      var typedScp       = new StaticSqlConnectionProvider("WS2016Sql", "WoeberLogisticsDbCopyWithMigrations");
      var table          = "Production.JobActivity";
      var newColumnName  = "TankInfo";
      var newColumnType  = "nvarchar(max)";
      var newColumnIndex = 15; //Zero-based column index

      var sqlTableRef  = new SqlTableReference(typedScp, "dbo", "Production.JobActivity");
      var columns      = InformationSchemaExplorer.GetSisColumns(sqlTableRef);
      var isTable      = InformationSchemaExplorer.GetSisTable(sqlTableRef);
      var tableDef     = InformationSchemaExplorer.GetSisTableDefinition(sqlTableRef);
      var constraints  = InformationSchemaExplorer.GetSisTableConstraints(sqlTableRef);
      var indexColumns = SysSchemaExplorer.GetIndexColumnDtos(sqlTableRef);

      indexColumns.PrintStringTable();
      constraints.PrintStringTable();

      ListExtensionMethods.MakeList(tableDef).PrintStringTable();
      tableDef.TableConstraints.PrintStringTable();
      ListExtensionMethods.MakeList(isTable).PrintStringTable();
      columns.PrintStringTable();
   }

   [Test]
   public async Task GenerateScriptWithoutPK()
   {
      var script = GenerateScript(new AddColumnVariables()
      {
         TableName      = "Production.JobActivity",
         ColumnName     = "TankInfo",
         ColumnType     = "nvarchar(max)",
         Nullability    = Nullability.NotNull,
         NewColumnIndex = 3,
         TableColumns = new List<Column>
         {
            new("Id", "int", Nullability.NotNull),
            new("FirstName", "nvarchar(20)", Nullability.NotNull),
            new("LastName", "nvarchar(200)", Nullability.NotNull),
            new("Age", "short", Nullability.Null),
         },
      });

      script.ToConsoleWriteLine();
   }

   public string GenerateScript(AddColumnVariables variables)
   {
      var tmpTableName = "tmp" + variables.TableName;

      var insertInto = InsertIntoTemplate
         .MyReplace("schema",       variables.Schema)
         .MyReplace("tmpTableName", tmpTableName)
         .MyReplace("tableName",    variables.TableName)
         .MyReplace("columnList",   variables.SelectColumnListPrior());

      var script = SqlTemplate
         .MyReplace("schema",        variables.Schema)
         .MyReplace("tmpTableName",  tmpTableName)
         .MyReplace("tableName",     variables.TableName)
         .MyReplace("newColumnList", variables.ColumnListWithNew())
         .MyReplace("insertIntoSql", insertInto);
      ;

      return script;
   }

   public class AddColumnVariables
   {
      public string      Schema         { get; set; } = "dbo";
      public string      TableName      { get; set; }
      public string      ColumnName     { get; set; }
      public string      ColumnType     { get; set; }
      public Nullability Nullability    { get; set; }
      public int         NewColumnIndex { get; set; }

      public List<IndexColumn> IndexColumns { get; set; }
      public List<Column>      TableColumns { get; set; }

      public string SelectColumnListPrior()
      {
         var selectColumns = TableColumns.Select(x => x.ColumnName).StringJoin(", ");
         return selectColumns;
      }

      public string ColumnListPre()
      {
         var allColumnsList = TableColumns
            .Select(x => $"{x.ColumnName} {x.ColumnType}" + (x.Nullability == Nullability.Null ? "NULL" : " NOT NULL"))
            .StringJoin(",\r\n");
         return allColumnsList;
      }

      public string ColumnListWithNew()
      {
         var columnsIncludingNew = TableColumns.ToList();
         columnsIncludingNew.Insert(NewColumnIndex, new Column(ColumnName, ColumnType, Nullability));
         var allColumnsList = columnsIncludingNew
            .Select(x => $"{x.ColumnName} {x.ColumnType}" + (x.Nullability == Nullability.Null ? " NULL" : " NOT NULL"))
            .StringJoin(",\r\n");
         return allColumnsList;
      }
   }

   public record Column(string ColumnName, string ColumnType, Nullability Nullability);

   public record IndexColumn(string Name, SortDirection SortDirection);

   public enum SortDirection
   {
      Asc,
      Desc
   }

   private static string InsertIntoTemplate = """
                                              INSERT INTO $schema$.[$tmpTableName$] ($columnList$)
                                                  SELECT $columnList$ FROM $schema$.[$tableName$] WITH(HOLDLOCK TABLOCKX)
                                              """;

   private static string SqlTemplate = """
                                       BEGIN TRANSACTION
                                       GO
                                       CREATE TABLE $schema$.[$tmpTableName$]
                                       (
                                       $newColumnList$
                                       )
                                       ALTER TABLE $schema$.[$tmpTableName$]
                                          SET (Lock_Escalation = TABLE)
                                       SET IDENTITY_INSERT $schema$.[$tmpTableName$] ON

                                       IF EXISTS(SELECT *
                                                 FROM $schema$.[$tableName$])
                                       EXEC ('$insertIntoSql$')
                                       GO
                                       SET IDENTITY_INSERT $schema$.[$tmpTableName$] OFF
                                       GO

                                       DROP TABLE $schema$.[$tableName$]
                                       GO

                                       EXECUTE sp_rename N'$schema$.[$tmpTableName$]', N'$tableName$', 'OBJECT'
                                       GO

                                       -- Add back the primary key information here:
                                       ALTER TABLE $schema$.[$tableName$]
                                          ADD CONSTRAINT
                                             [$primaryKeyConstraintName$] PRIMARY KEY CLUSTERED
                                                (
                                                   $primaryKeyColumns$
                                                )

                                       GO
                                       COMMIT

                                       """;
}

public static class ConsoleExtensions
{
   public static void ToConsoleWriteLine(this string s)
   {
      Console.WriteLine(s);
   }
}