namespace UniqueDb.ConnectionProvider.SqlMetadata;

public class MainToHistoryTableLink
{
   public string? MainSchemaName    { get; set; }
   public string  MainTableName     { get; set; }
   public string? HistorySchemaName { get; set; }
   public string? HistoryTableName  { get; set; }

   public static string SqlQuery()
   {
      var sql =
         """
         SELECT SCHEMA_NAME(t.schema_id) AS MainSchemaName,
         t.name                   AS MainTableName,
         SCHEMA_NAME(h.schema_id) AS HistorySchemaName,
         h.name                   AS HistoryTableName,
         t.*
         FROM sys.tables t
          LEFT JOIN sys.tables h ON t.history_table_id = h.object_id
         ORDER BY SCHEMA_NAME(t.schema_id), t.name
         """;
      return sql;
   }
}