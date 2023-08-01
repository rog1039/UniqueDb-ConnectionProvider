namespace UniqueDb.ConnectionProvider.SqlMetadata.SysTables;

public class SysIndex
{
   public string? SchemaName                  { get; set; }
   public string  TableName                   { get; set; }
   public string? IndexName                   { get; set; }
   public int     object_id                   { get; set; }
   public string? name                        { get; set; }
   public int     index_id                    { get; set; }
   public byte    type                        { get; set; }
   public string? type_desc                   { get; set; }
   public bool?   is_unique                   { get; set; }
   public int?    data_space_id               { get; set; }
   public bool?   ignore_dup_key              { get; set; }
   public bool?   is_primary_key              { get; set; }
   public bool?   is_unique_constraint        { get; set; }
   public byte    fill_factor                 { get; set; }
   public bool?   is_padded                   { get; set; }
   public bool?   is_disabled                 { get; set; }
   public bool?   is_hypothetical             { get; set; }
   public bool?   is_ignored_in_optimization  { get; set; }
   public bool?   allow_row_locks             { get; set; }
   public bool?   allow_page_locks            { get; set; }
   public bool?   has_filter                  { get; set; }
   public string? filter_definition           { get; set; }
   public int?    compression_delay           { get; set; }
   public bool?   suppress_dup_key_messages   { get; set; }
   public bool?   auto_created                { get; set; }
   public bool?   optimize_for_sequential_key { get; set; }

   public const string SqlQuery =
      """
      SELECT
          SCHEMA_NAME(o.schema_id) AS SchemaName,
          o.name                   AS TableName,
          i.name                   AS IndexName,
          i.*
      FROM sys.indexes          i
               JOIN sys.objects o ON i.object_id = o.object_id
      ORDER BY SchemaName, TableName, index_id
      """;
}