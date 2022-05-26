namespace UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;

[Obsolete($"Replaced by {nameof(TemporalTableScriptCreator)}")]
public static class TemporalTableDmlCreator
{
   public static string GetCreateTableScriptTemporal(SqlTable         table,
                                                     IList<SqlColumn> pkNames,
                                                     string?          tableSchema = null,
                                                     string?          tableName   = null)
   {
      tableName   ??= table.Name;
      tableSchema ??= table.Schema;

      var columnText               = CreateTableColumnsText(table, pkNames);
      var primaryKeyConstraintText = CreatePrimaryKeyConstraintText(table, pkNames, tableName);

      var temporalColumnsText = primaryKeyConstraintText.IsNullOrWhitespace()
         ? string.Empty
         : @",ValidFrom datetime2 GENERATED ALWAYS AS ROW START,
ValidTo datetime2 GENERATED ALWAYS AS ROW END ,
PERIOD FOR SYSTEM_TIME ( ValidFrom,ValidTo ),";

      var historyTableName = $"{tableName}_History".BracketizeSafe();
      var historyTableText = primaryKeyConstraintText.IsNullOrWhitespace()
         ? string.Empty
         : $"WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = {tableSchema.BracketizeSafe()}.{historyTableName}))";

      var script = $@"
CREATE TABLE [{tableSchema}].[{tableName}] (
{columnText}
{temporalColumnsText}
{primaryKeyConstraintText}
)
{historyTableText}
;
";
      return script;
   }

   private static object CreateTableColumnsText(SqlTable table, IList<SqlColumn> pkNames)
   {
      var columnText = table
         .SqlColumns
         .OrderBy(z => z.OrdinalPosition)
         .Select(col =>
         {
            var columnName = $"[{col.Name}]";
            var sqlType    = col.SqlDataType;
            return $"  {columnName,-20} {sqlType,-20} {GetColumnNullableText(pkNames, col),-10}";
         })
         .StringJoin("," + Environment.NewLine);
      return columnText;
   }

   private static string GetColumnNullableText(IList<SqlColumn> pkNames, SqlColumn column)
   {
      var isPkColumn = pkNames.Any(pk => pk.Name.InsensitiveEquals(column.Name));
      return column.IsNullable || isPkColumn
         ? "NOT NULL"
         : "NULL";
   }

   private static string CreatePrimaryKeyConstraintText(SqlTable table, IList<SqlColumn> pkNames, string tableName)
   {
      var primaryKeyColumnsText = pkNames
         .Select(z => z.Name.BracketizeSafe())
         .StringJoin(", ");
      return primaryKeyColumnsText.IsNullOrWhitespace()
         ? String.Empty
         : $"CONSTRAINT PK_{tableName} PRIMARY KEY CLUSTERED ({primaryKeyColumnsText})";
   }
}