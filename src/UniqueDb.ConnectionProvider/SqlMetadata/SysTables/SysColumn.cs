namespace UniqueDb.ConnectionProvider.SqlMetadata.SysTables;

public class SysColumn
{
   public string? SchemaName                          { get; set; }
   public string  TableName                           { get; set; }
   public string? ColumnName                          { get; set; }
   public string  TypeName                            { get; set; }
   public int     object_id                           { get; set; }
   public string? name                                { get; set; }
   public int     column_id                           { get; set; }
   public byte    system_type_id                      { get; set; }
   public int     user_type_id                        { get; set; }
   public short   max_length                          { get; set; }
   public byte    precision                           { get; set; }
   public byte    scale                               { get; set; }
   public string? collation_name                      { get; set; }
   public bool?   is_nullable                         { get; set; }
   public bool    is_ansi_padded                      { get; set; }
   public bool    is_rowguidcol                       { get; set; }
   public bool    is_identity                         { get; set; }
   public bool?   is_computed                         { get; set; }
   public bool    is_filestream                       { get; set; }
   public bool?   is_replicated                       { get; set; }
   public bool?   is_non_sql_subscribed               { get; set; }
   public bool?   is_merge_published                  { get; set; }
   public bool?   is_dts_replicated                   { get; set; }
   public bool    is_xml_document                     { get; set; }
   public int     xml_collection_id                   { get; set; }
   public int     default_object_id                   { get; set; }
   public int     rule_object_id                      { get; set; }
   public bool?   is_sparse                           { get; set; }
   public bool?   is_column_set                       { get; set; }
   public byte?   generated_always_type               { get; set; }
   public string? generated_always_type_desc          { get; set; }
   public int?    encryption_type                     { get; set; }
   public string? encryption_type_desc                { get; set; }
   public string? encryption_algorithm_name           { get; set; }
   public int?    column_encryption_key_id            { get; set; }
   public string? column_encryption_key_database_name { get; set; }
   public bool?   is_hidden                           { get; set; }
   public bool    is_masked                           { get; set; }
   public int?    graph_type                          { get; set; }
   public string? graph_type_desc                     { get; set; }
   public bool?   is_data_deletion_filter_column      { get; set; }
   public int?    ledger_view_column_type             { get; set; }
   public string? ledger_view_column_type_desc        { get; set; }
   public bool?   is_dropped_ledger_column            { get; set; }

   public static readonly string Query =
      """
      SELECT SCHEMA_NAME(t.schema_id) AS SchemaName,
       t.name                   AS TableName,
       c.name                   AS ColumnName,
      --        o.name      AS ObjectName,
      --        o.type      AS OType,
      --        o.type_desc AS OTypeDesc,
       type.name                AS TypeName,
       c.*
      FROM sys.columns c
         --          JOIN sys.objects o ON c.object_id = o.object_id
         --Note: Sys.Objects and sys.Columns contain SYSTEM tables and columns, which we don't really care about.
         --      Joining on sys.tables removes all those since sys.Tables does not contain the system tables.
         --      A left join on sys.tables will keep those system objects.
         JOIN sys.tables t ON c.object_id = t.object_id
      --     Also, worth keeping in mind how to join across the sys.types table.
      --     See here for explanation: https://stackoverflow.com/questions/20348522/in-sql-server-what-is-the-difference-between-user-type-id-and-system-type-id-in
      --     Summary:
      --     1:) sys.columns.user_type_id = sys.types.user_type_id
      --            For a built-in type, it returns the built-in type.
      --            For a user-defined type, it returns the user-defined type.
      --     2:) sys.columns.system_type_id = sys.types.user_type_id
      --            For a built-in type, it returns the built-in type.
      --            For a user-defined type, it returns the built-in base type. This might make sense, for example, if you want to get all varchar columns, including all user-defined columns based on varchar.
         JOIN sys.types type ON c.system_type_id = type.user_type_id
      --WHERE
      ORDER BY SchemaName, TableName, column_id
      """;

   /// <summary>
   /// Containers query parameters @schemaName and @tableName
   /// </summary>
   /// <returns></returns>
   public static string QueryForTable()
   {
      return Query.Replace(
         "--WHERE",
         "WHERE SCHEMA_NAME(t.schema_id) = @schemaName AND t.Name = @tableName");
   }
}