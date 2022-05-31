using UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public class SqlTableForTemporal
{
   public string Schema { get; set; }
   public string Name   { get; set; }

   public IList<SqlColumnForTemporal> Columns { get; set; } = new List<SqlColumnForTemporal>();

   public bool HasPrimaryKey() => Columns.Any(z => z.IsPrimaryKey == true);

   public IList<IndexAnnotation> IndexAnnotations { get; set; } = new List<IndexAnnotation>();
}

public class SqlColumnForTemporal
{
   public string  Name            { get; set; }
   public int     OrdinalPosition { get; set; }
   public SqlType SqlType         { get; set; }
   public bool    IsNullable      { get; set; }
   public bool?   IsIdentity      { get; set; }
   public bool?   IsPrimaryKey    { get; set; }
}

public static class TemporalTableScriptCreator
{
   /// <summary>
   /// Boilerplate text to create Temporal table ValidFrom,ValidTo columns.
   /// </summary>
   private const string TemporalColumnsText = @",
ValidFrom   datetime2 GENERATED ALWAYS AS ROW START,
ValidTo     datetime2 GENERATED ALWAYS AS ROW END ,
PERIOD FOR SYSTEM_TIME (ValidFrom,ValidTo),";

   /// <summary>
   /// Returns the SQL create script to create the temporal table requested.
   /// </summary>
   /// <param name="sqlTable"></param>
   /// <param name="schemaName"></param>
   /// <param name="tableName"></param>
   /// <returns></returns>
   public static string GetScript(SqlTableForTemporal sqlTable,
                                  string              schemaName = "dbo",
                                  string?             tableName  = null)
   {
      tableName ??= sqlTable.Name;

      var dbTableName          = new DbTableName(schemaName, tableName);
      var regularColumns       = CreateNormalColumns(sqlTable);
      var temporalColumns      = GetTemporalColumns(sqlTable);
      var primaryKeyConstraint = CreatePrimaryKeyConstraint(sqlTable, tableName);
      var historyTableLink     = CreateHistoryTableLink(sqlTable, schemaName, tableName);
      var indexCreation        = CreateIndexText(sqlTable, dbTableName);

      /*
       * A SQL temporal table is made up of:
       *  - the regular columns of the table
       *  - the required temporal columns for validto/validfrom
       *  - temporal tables won't create without a primary key constraint
       *  - history table linkage and naming.
       */
      var script = $@"
CREATE TABLE {dbTableName} (
{regularColumns}{temporalColumns}
{primaryKeyConstraint}
)
{historyTableLink};

{indexCreation}"
         .RemoveEmptyLines();
      return script;
   }

   private static string CreateIndexText(SqlTableForTemporal sqlTableForTemporal, DbTableName dbTableName)
   {
      if(sqlTableForTemporal.IndexAnnotations.Count == 0) return String.Empty;
      
      var newIndexStatements = sqlTableForTemporal
         .IndexAnnotations
         .Select(ia => IndexDmlScriptCreator.GetScript(ia))
         .StringJoin(Environment.NewLine);
      return $"{newIndexStatements}";
   }

   /// <summary>
   /// Generates the script portion to create the non-temporal columns of the table.
   /// </summary>
   /// <param name="sqlTable"></param>
   /// <returns></returns>
   private static string CreateNormalColumns(SqlTableForTemporal sqlTable)
   {
      var columnText = sqlTable
         .Columns
         .OrderBy(z => z.OrdinalPosition)
         .Select(col =>
         {
            var columnName    = col.Name.BracketizeSafe();
            var sqlType       = col.SqlType;
            var isNotNullable = col.IsPrimaryKey == true || col.IsNullable == false;
            var nullableText  = isNotNullable ? "NOT NULL" : "NULL";

            return $"  {columnName,-25} {sqlType,-20} {nullableText,-10}";
         })
         .StringJoinCommaNewLine();
      return columnText;
   }

   private static string GetTemporalColumns(SqlTableForTemporal sqlTable)
   {
      return sqlTable.HasPrimaryKey() 
         ? TemporalColumnsText 
         : string.Empty;
   }

   /// <summary>
   /// Gather all primary key columns and use them to create the PK constraint on the table.
   /// </summary>
   /// <param name="sqlTable"></param>
   /// <param name="tableName"></param>
   /// <returns></returns>
   private static string CreatePrimaryKeyConstraint(SqlTableForTemporal sqlTable, string tableName)
   {
      var primaryKeyColumnsText = sqlTable
         .Columns
         .Where(z => z.IsPrimaryKey == true)
         .Select(z => z.Name.BracketizeSafe())
         .StringJoin(", ");
      return primaryKeyColumnsText.IsNotNullOrWhitespace()
         ? $"CONSTRAINT PK_{tableName} PRIMARY KEY CLUSTERED ({primaryKeyColumnsText})"
         : string.Empty;
   }

   /// <summary>
   /// Creates a history table link if there is a primary key on the main table.
   /// </summary>
   /// <param name="sqlTable"></param>
   /// <param name="schemaName"></param>
   /// <param name="tableName"></param>
   /// <returns></returns>
   private static string CreateHistoryTableLink(SqlTableForTemporal sqlTable, string schemaName, string tableName)
   {
      if (!sqlTable.HasPrimaryKey()) return string.Empty;

      var historyTableName = $"{tableName}_History".BracketizeSafe();
      var dbTableName      = new DbTableName(schemaName, historyTableName);
      return $"WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = {dbTableName}))";
   }
}