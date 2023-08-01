namespace UniqueDb.ConnectionProvider.SqlMetadata.SysTables;

public class SysTable
{
   public string?  SchemaName                         { get; set; }
   public string   TableName                          { get; set; }
   public string?  HistorySchemaName                  { get; set; }
   public string?  HistoryTableName                   { get; set; }
   public string   name                               { get; set; }
   public int      object_id                          { get; set; }
   public int?     principal_id                       { get; set; }
   public int      schema_id                          { get; set; }
   public int      parent_object_id                   { get; set; }
   public string?  type                               { get; set; }
   public string?  type_desc                          { get; set; }
   public DateTime create_date                        { get; set; }
   public DateTime modify_date                        { get; set; }
   public bool     is_ms_shipped                      { get; set; }
   public bool     is_published                       { get; set; }
   public bool     is_schema_published                { get; set; }
   public int      lob_data_space_id                  { get; set; }
   public int?     filestream_data_space_id           { get; set; }
   public int      max_column_id_used                 { get; set; }
   public bool     lock_on_bulk_load                  { get; set; }
   public bool?    uses_ansi_nulls                    { get; set; }
   public bool?    is_replicated                      { get; set; }
   public bool?    has_replication_filter             { get; set; }
   public bool?    is_merge_published                 { get; set; }
   public bool?    is_sync_tran_subscribed            { get; set; }
   public bool     has_unchecked_assembly_data        { get; set; }
   public int?     text_in_row_limit                  { get; set; }
   public bool?    large_value_types_out_of_row       { get; set; }
   public bool?    is_tracked_by_cdc                  { get; set; }
   public byte?    lock_escalation                    { get; set; }
   public string?  lock_escalation_desc               { get; set; }
   public bool?    is_filetable                       { get; set; }
   public bool?    is_memory_optimized                { get; set; }
   public byte?    durability                         { get; set; }
   public string?  durability_desc                    { get; set; }
   public byte?    temporal_type                      { get; set; }
   public string?  temporal_type_desc                 { get; set; }
   public int?     history_table_id                   { get; set; }
   public bool?    is_remote_data_archive_enabled     { get; set; }
   public bool     is_external                        { get; set; }
   public int?     history_retention_period           { get; set; }
   public int?     history_retention_period_unit      { get; set; }
   public string?  history_retention_period_unit_desc { get; set; }
   public bool?    is_node                            { get; set; }
   public bool?    is_edge                            { get; set; }
   public int?     data_retention_period              { get; set; }
   public int?     data_retention_period_unit         { get; set; }
   public string?  data_retention_period_unit_desc    { get; set; }
   public byte?    ledger_type                        { get; set; }
   public string?  ledger_type_desc                   { get; set; }
   public int?     ledger_view_id                     { get; set; }
   public bool?    is_dropped_ledger_table            { get; set; }

   public const string Query =
      """
      SELECT SCHEMA_NAME(t.schema_id) AS SchemaName,
        t.name                   AS TableName,
        SCHEMA_NAME(h.schema_id) AS HistorySchemaName,
        h.name                   AS HistoryTableName,
        t.*
      FROM sys.tables AS t
         LEFT JOIN sys.tables h ON t.history_table_id = h.object_id
      --WHERE
      ORDER BY SchemaName, TableName
      """;

   public static string QueryForTable = Query.Replace(
      "--WHERE",
      "WHERE SCHEMA_NAME(t.schema_id) = @schemaName AND t.name = @tableName");
}

public class SysTableDefinition
{
   public SysTable  SysTable  { get; set; }
   public SysColumn SysColumn { get; set; }
   
}