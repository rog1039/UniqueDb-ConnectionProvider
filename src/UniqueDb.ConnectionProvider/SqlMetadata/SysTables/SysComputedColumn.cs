namespace UniqueDb.ConnectionProvider.SqlMetadata.SysTables;

public class SysComputedColumn
{
   public string? SchemaName                          { get; set; }
   public string? TableName                           { get; set; }
   public string? ColumnName                          { get; set; }
   public int     object_id                           { get; set; }
   public bool?   is_computed                         { get; set; }
   public string? definition                          { get; set; }
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
   public bool    is_filestream                       { get; set; }
   public bool?   is_replicated                       { get; set; }
   public bool?   is_non_sql_subscribed               { get; set; }
   public bool?   is_merge_published                  { get; set; }
   public bool?   is_dts_replicated                   { get; set; }
   public bool    is_xml_document                     { get; set; }
   public int     xml_collection_id                   { get; set; }
   public int     default_object_id                   { get; set; }
   public int     rule_object_id                      { get; set; }
   public bool    uses_database_collation             { get; set; }
   public bool    is_persisted                        { get; set; }
   public bool    is_sparse                           { get; set; }
   public bool    is_column_set                       { get; set; }
   public byte?   generated_always_type               { get; set; }
   public string? generated_always_type_desc          { get; set; }
   public int?    encryption_type                     { get; set; }
   public string? encryption_type_desc                { get; set; }
   public string? encryption_algorithm_name           { get; set; }
   public int?    column_encryption_key_id            { get; set; }
   public string? column_encryption_key_database_name { get; set; }
   public bool    is_hidden                           { get; set; }
   public bool    is_masked                           { get; set; }
   public int?    graph_type                          { get; set; }
   public string? graph_type_desc                     { get; set; }
   public bool?   is_data_deletion_filter_column      { get; set; }
   public int?    ledger_view_column_type             { get; set; }
   public string? ledger_view_column_type_desc        { get; set; }
   public bool?   is_dropped_ledger_column            { get; set; }
}

public class ComputedColumnQuery
{
   public static string Query =
      """
      SELECT c.object_id,
      SCHEMA_NAME(o.schema_id) AS SchemaName,
      OBJECT_NAME(o.object_id) AS TableName,
      c.name                   AS ColumnName,
      c.is_computed,
      c.definition,
      c.column_id,
      c.system_type_id,
      c.user_type_id,
      c.max_length,
      c.precision,
      c.scale,
      c.collation_name,
      c.is_nullable,
      c.is_ansi_padded,
      c.is_rowguidcol,
      c.is_identity,
      c.is_filestream,
      c.is_replicated,
      c.is_non_sql_subscribed,
      c.is_merge_published,
      c.is_dts_replicated,
      c.is_xml_document,
      c.xml_collection_id,
      c.default_object_id,
      c.rule_object_id,
      c.uses_database_collation,
      c.is_persisted,
      c.is_sparse,
      c.is_column_set,
      c.generated_always_type,
      c.generated_always_type_desc,
      c.encryption_type,
      c.encryption_type_desc,
      c.encryption_algorithm_name,
      c.column_encryption_key_id,
      c.column_encryption_key_database_name,
      c.is_hidden,
      c.is_masked,
      c.graph_type,
      c.graph_type_desc,
      c.is_data_deletion_filter_column,
      c.ledger_view_column_type,
      c.ledger_view_column_type_desc,
      c.is_dropped_ledger_column
      FROM sys.computed_columns c
      JOIN sys.objects o ON c.object_id = o.object_id
      --WHERE
      """;
}