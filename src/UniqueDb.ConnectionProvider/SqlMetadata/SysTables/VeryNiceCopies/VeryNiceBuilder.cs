using System.Data.Common;
using Dapper;
using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.SqlScriptingEngine.Attempt2;

namespace UniqueDb.ConnectionProvider.SqlMetadata.SysTables.VeryNiceCopies;

public static class VeryNiceBuilder
{
   public static async Task<TableSpec> BuildTableSpec(SqlTableReference sqlTableReference)
   {
      /*
       * How many queries do we need?
       * Need SysTable
       * Need SysColumns -> Identity/Computed
       * Need SysIndexes
       * Need SysColumns
       * Need ForeignKeys
       * Need ForeignKeyColumns
       */

      var sysTableNicer = await sqlTableReference.GetSysTablesExact();

      var tableSpec = new TableSpec()
      {
         SchemaName        = sysTableNicer.SchemaName,
         TableName         = sysTableNicer.TableName,
         TemporalType      = TemporalTypeFromNumeric(sysTableNicer.temporal_type.Value),
         IsMemoryOptimized = sysTableNicer.is_memory_optimized.Value,
      };
      tableSpec.ColumnSpecs = await GetColumnSpecs(sqlTableReference);
      return tableSpec;
   }

   public static TemporalType TemporalTypeFromNumeric(int temporalType) => temporalType switch
   {
      0 => TemporalType.None,
      1 => TemporalType.HistoryTable,
      2 => TemporalType.SystemVersionedTable,
   };

   public static async Task<List<ColumnSpec>> GetColumnSpecs(SqlTableReference sqlTableReference)
   {
      var columns = await sqlTableReference.GetSysColumnsNicer();
      var columnSpecs = columns
         .Select(x =>
         {
            //
            return new ColumnSpec()
            {
               SchemaName     = x.SchemaName,
               TableName      = x.TableName,
               ColumnName     = x.ColumnName,
               IsComputed     = x.is_computed == true,
               IsIdentity     = x.is_identity == true,
               IsNullable     = x.is_nullable == true,
               SqlType        = SqlTypeFactory.Create(x.TypeName, x.precision, x.scale, x.max_length),
               ColumnId       = x.column_id,
               ObjectId       = x.object_id,
               SystemTypeId   = x.system_type_id,
               UserTypeId     = x.user_type_id,
               Definition     = x.definition,
               IsPersisted    = x.is_persisted,
               SeedValue      = x.seed_value,
               IncrementValue = x.increment_value,
               LastValue      = x.last_value,
               GeneratedAlwaysType = x.generated_always_type switch
               {
                  1 => TemporalColumnType.Start,
                  2 => TemporalColumnType.End,
                  _ => TemporalColumnType.None,
               },
            };
         })
         .ToList();
      return columnSpecs;
   }

   public static async Task<IList<IndexColumnSpec>> GetIndexColumnSpecs(SqlTableReference sqlTableReference)
   {
      var indexColumnNicers = await sqlTableReference.GetSysIndexColumnsNicer();
      var indexColumnSpecs = indexColumnNicers.Select(x =>
         {
            return new IndexColumnSpec()
            {
               SchemaName        = x.SchemaName,
               TableName         = x.TableName,
               ColumnName        = x.ColumnName,
               ColumnId          = x.column_id,
               IndexName         = x.IndexName,
               IsNullable        = x.is_nullable == true,
               SortDirection     = x.is_descending_key == true ? SortDirection.Desc : SortDirection.Asc,
               KeyOrdinal        = x.key_ordinal,
               IndexColumnNumber = x.index_column_id,
               IsIncludedColumn  = x.is_included_column == true,
               SqlType           = SqlTypeFactory.Create(x.TypeName, x.precision, x.scale, x.max_length),
               IndexId = x.index_id,
            };
         })
         .ToList();
      return indexColumnSpecs;
   }

   public static async Task<IList<IndexSpec>> GetIndexSpec(this SqlTableReference sqlTableReference)
   {
      var indexes      = await sqlTableReference.GetSysIndexesNicer();
      var indexColumns = await sqlTableReference.GetIndexColumnSpecs();
      var indexSpecs = indexes.Select(x =>
      {
         return new IndexSpec()
         {
            SchemaName = x.SchemaName,
            TableName  = x.TableName,
            IndexName  = x.IndexName,
            IsUnique   = x.IsUnique == true,
            IndexType = GetIndexTypeFromNumber(x.type),
            IsPrimaryKey       = x.IsPrimaryKey == true,
            IsUniqueConstraint = x.IsUniqueConstraint == true,
            IndexId = x.index_id,
            IndexColumnSpecs = indexColumns.Where(y => y.IndexId == x.index_id).ToList(),
         };
      }).ToList();
      return indexSpecs;
   }

   private static IndexType GetIndexTypeFromNumber(int indexTypeNumber)
   {
      return indexTypeNumber switch
      {
         0 => IndexType.Heap,
         1 => IndexType.Clustered,
         2 => IndexType.NonClustered,
         3 => IndexType.XML,
         4 => IndexType.Spatial,
         5 => IndexType.ClusteredColumnStore,
         6 => IndexType.NonClusteredColumnStore,
         7 => IndexType.NonClusteredHashIndex,
         _ => throw new ArgumentOutOfRangeException(nameof(indexTypeNumber), indexTypeNumber, null)
      };
   }
}

public static class ScpSysSchemaExtensions
{
   public static async Task<SysTableNicer> GetSysTablesExact(this SqlTableReference sqlTableRef)
   {
      var tables = await sqlTableRef.MyQueryAsync<SysTableNicer>(
         """
         SELECT
         SCHEMA_NAME(t.schema_id) as SchemaName,
         t.name as TableName,
         t.*
         FROM sys.tables t
         WHERE schema_name(t.schema_id) = @schema and t.name = @table
         """,
         sqlTableRef.ToTableName()
      );
      return tables.First();
   }

   public static async Task<IList<SysColumnNicer>> GetSysColumnsNicer(this SqlTableReference sqlTableRef)
   {
      var columns = await sqlTableRef.MyQueryAsync<SysColumnNicer>(
         """
         SELECT
         o.type_desc              AS ObjectType,
         SCHEMA_NAME(o.schema_id) AS SchemaName,
         o.name                   AS TableName,
         c.name                   AS ColumnName,
         --     c.is_nullable,
         --     c.precision,
         --     c.scale,
         --     c.max_length,

         type.name as TypeName,

         cc.definition,
         cc.is_persisted,
         cc.uses_database_collation,

         ic.increment_value,
         ic.is_not_for_replication,
         ic.last_value,
         ic.seed_value,

         type.scale as TypeScale,
         type.precision as TypePrecision,
         type.max_length as TypeMaxLength,
         type.is_nullable            TypeIsNullable,
         type.is_user_defined        TypeIsUserDefined,

         c.*
         FROM sys.columns                        c
         JOIN      sys.objects          o ON o.object_id = c.object_id
         LEFT JOIN sys.computed_columns cc ON cc.object_id = c.object_id
         AND cc.column_id = c.column_id
         LEFT JOIN sys.identity_columns ic ON ic.object_id = c.object_id
         AND ic.column_id = c.column_id
         JOIN      sys.types            type ON c.system_type_id = type.user_type_id
         WHERE o.type <> 'S'
         AND o.type <> 'IT'
         --   AND (type.name = 'decimal' OR type.name = 'int')
         --   AND SCHEMA_NAME(o.schema_id) <> 'PbsiSF'
         --   AND SCHEMA_NAME(o.schema_id) <> 'PbsiWM'
         AND SCHEMA_NAME(o.schema_id) = @schema
         AND o.name = @table
         """, sqlTableRef.ToTableName());
      return columns;
   }

   public static async Task<IList<SysIndexNicer>> GetSysIndexesNicer(this SqlTableReference sqlTableReference)
   {
      var indexes = await sqlTableReference.MyQueryAsync<SysIndexNicer>(
         """
         SELECT
         SCHEMA_NAME(o.schema_id)      AS SchemaName,
         o.name                        AS TableName,
         i.name                        AS IndexName,
         i.fill_factor                 AS 'FillFactor',
         i.is_primary_key              AS IsPrimaryKey,
         i.is_unique                   AS IsUnique,
         i.is_unique_constraint        AS IsUniqueConstraint,
         i.optimize_for_sequential_key AS OptimizeForSequentialKey,
         i.allow_page_locks,
         i.allow_row_locks,
         i.auto_created,
         i.compression_delay,
         i.data_space_id,
         i.filter_definition,
         i.has_filter,
         i.ignore_dup_key,
         i.is_disabled,
         i.is_hypothetical,
         i.is_ignored_in_optimization,
         i.is_padded,
         i.suppress_dup_key_messages,
         i.type,
         i.type_desc,
         i.index_id
         FROM sys.indexes          i
         JOIN sys.objects o ON o.object_id = i.object_id
         WHERE o.type <> 'S'
         AND o.type <> 'IT'
         --   AND SCHEMA_NAME(o.schema_id) <> 'PbsiWM'
         --   AND SCHEMA_NAME(o.schema_id) <> 'PbsiSF'
         AND SCHEMA_NAME(o.schema_id) = @schema
         AND o.name = @table
         ORDER BY SCHEMA_NAME(o.schema_id),
         o.name, i.name
         """,
         sqlTableReference.ToTableName());
      return indexes;
   }

   public static async Task<IList<SysIndexColumnNicer>> GetSysIndexColumnsNicer(this SqlTableReference sqlTableRef)
   {
      var columns = await sqlTableRef.MyQueryAsync<SysIndexColumnNicer>(
         """
         SELECT
         SCHEMA_NAME(o.schema_id) AS SchemaName,
         o.name                   AS TableName,
         c.name                   AS ColumnName,
         c.column_id,
         ic.index_column_id,
         ic.key_ordinal,
         ic.is_descending_key,
         ic.is_included_column,
         t.name,
         c.is_nullable,
         c.precision,
         c.scale,
         c.max_length,
         ic.column_store_order_ordinal,
         ic.index_id,
         ic.partition_ordinal,
         i.name                   AS IndexName,
         t.name                   AS TypeName
         FROM sys.index_columns    ic
         JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
         JOIN sys.objects o ON ic.object_id = o.object_id
         JOIN sys.types   t ON c.system_type_id = t.user_type_id
         JOIN sys.indexes i ON i.index_id = ic.index_id AND i.object_id = ic.object_id
         WHERE o.type <> 'S'
           AND o.type <> 'IT'
           AND SCHEMA_NAME(o.schema_id) = @schema
           AND o.name = @table
         """,
         sqlTableRef.ToTableName());
      return columns;
   }

   public static async Task<IList<IndexColumnSpec>> GetIndexColumnSpecs(this SqlTableReference sqlTableReference)
   {
      return await VeryNiceBuilder.GetIndexColumnSpecs(sqlTableReference);
   }

   public static Task<IList<T>> MyQueryAsync<T>(this SqlTableReference sqlTableReference, string query,
                                                object                 param = null)
   {
      return sqlTableReference.SqlConnectionProvider.MyQueryAsync<T>(query, param);
   }

   public static async Task<IList<T>> MyQueryAsync<T>(this ISqlConnectionProvider scp, string query,
                                                      object                      param = null)
   {
      var conn = scp.GetSqlConnection();
      return await conn.MyQueryAsync<T>(query, param);
   }

   public static async Task<IList<T>> MyQueryAsync<T>(this DbConnection conn, string query, object param = null)
   {
      var resultEnumerable = await conn.QueryAsync<T>(query, param);
      var resultList       = resultEnumerable.ToList();
      return resultList;
   }
}

public class SysIndexColumnNicer
{
   public string? SchemaName                 { get; set; }
   public string  TableName                  { get; set; }
   public string? ColumnName                 { get; set; }
   public int     column_id                  { get; set; }
   public int     index_column_id            { get; set; }
   public byte    key_ordinal                { get; set; }
   public bool?   is_descending_key          { get; set; }
   public bool?   is_included_column         { get; set; }
   public string  name                       { get; set; }
   public bool?   is_nullable                { get; set; }
   public byte    precision                  { get; set; }
   public byte    scale                      { get; set; }
   public short   max_length                 { get; set; }
   public byte    column_store_order_ordinal { get; set; }
   public int     index_id                   { get; set; }
   public byte    partition_ordinal          { get; set; }
   public string? IndexName                  { get; set; }
   public string  TypeName                   { get; set; }
}

public class SysTableNicer
{
   public string?  SchemaName                         { get; set; }
   public string   TableName                          { get; set; }
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
}

public class SysColumnNicer
{
   public string? ObjectType                          { get; set; }
   public string? SchemaName                          { get; set; }
   public string  TableName                           { get; set; }
   public string? ColumnName                          { get; set; }
   public string  TypeName                            { get; set; }
   public string? definition                          { get; set; }
   public bool?   is_persisted                        { get; set; }
   public bool?   uses_database_collation             { get; set; }
   public object? increment_value                     { get; set; }
   public bool?   is_not_for_replication              { get; set; }
   public object? last_value                          { get; set; }
   public object? seed_value                          { get; set; }
   public byte    TypeScale                           { get; set; }
   public byte    TypePrecision                       { get; set; }
   public short   TypeMaxLength                       { get; set; }
   public bool?   TypeIsNullable                      { get; set; }
   public bool    TypeIsUserDefined                   { get; set; }
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
}

public class SysIndexNicer
{
   public string? SchemaName                 { get; set; }
   public string  TableName                  { get; set; }
   public string? IndexName                  { get; set; }
   public byte    FillFactor                 { get; set; }
   public bool?   IsPrimaryKey               { get; set; }
   public bool?   IsUnique                   { get; set; }
   public bool?   IsUniqueConstraint         { get; set; }
   public bool?   OptimizeForSequentialKey   { get; set; }
   public bool?   allow_page_locks           { get; set; }
   public bool?   allow_row_locks            { get; set; }
   public bool?   auto_created               { get; set; }
   public int?    compression_delay          { get; set; }
   public int?    data_space_id              { get; set; }
   public string? filter_definition          { get; set; }
   public bool?   has_filter                 { get; set; }
   public bool?   ignore_dup_key             { get; set; }
   public bool?   is_disabled                { get; set; }
   public bool?   is_hypothetical            { get; set; }
   public bool?   is_ignored_in_optimization { get; set; }
   public bool?   is_padded                  { get; set; }
   public bool?   suppress_dup_key_messages  { get; set; }
   public byte    type                       { get; set; }
   public string? type_desc                  { get; set; }
   public int     index_id                    { get; set; }
}