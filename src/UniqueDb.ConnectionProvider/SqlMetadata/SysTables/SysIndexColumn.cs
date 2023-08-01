using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider.SqlMetadata.SysTables;

public class SysIndexColumn
{
   public int     index_id             { get; set; }
   public int     column_id            { get; set; }
   public int     index_column_id      { get; set; }
   public string? SchemaName           { get; set; }
   public string? TableName            { get; set; }
   public string? Index_Name           { get; set; }
   public string? Column_Name          { get; set; }
   public bool?   is_descending_key    { get; set; }
   public byte    key_ordinal          { get; set; }
   public bool?   is_included_column   { get; set; }
   public byte    IndexType            { get; set; }
   public string? IndexTypeDesc        { get; set; }
   public bool?   is_unique            { get; set; }
   public int?    data_space_id        { get; set; }
   public bool?   ignore_dup_key       { get; set; }
   public bool?   is_primary_key       { get; set; }
   public bool?   is_unique_constraint { get; set; }
   public byte    fill_factor          { get; set; }
   public bool?   is_padded            { get; set; }
   public bool?   is_disabled          { get; set; }
   public bool?   auto_created         { get; set; }

   public static string GetSqlQuery(SqlTableReference sqlTableReference)
   {
      return GetSqlQuery(sqlTableReference.SchemaName, sqlTableReference.TableName);
   }

   public static string GetSqlQuery(string schema, string table)
   {
      return SqlQuery
            .MyReplace("schema",    schema)
            .MyReplace("tableName", table)
         ;
   }

   public const string SqlQuery =
      """
        SELECT
            ic.index_id,
            ic.column_id,
            ic.index_column_id,
            SCHEMA_NAME(o.schema_id)             AS SchemaName,
            OBJECT_NAME(i.object_id)             AS TableName,
            i.name                               AS Index_Name,
            COL_NAME(ic.object_id, ic.column_id) AS Column_Name,
            ic.is_descending_key,
            ic.key_ordinal,
            ic.is_included_column,
            i.type                                  IndexType,
            i.type_desc                             IndexTypeDesc,
            i.is_unique,
            i.data_space_id,
            i.ignore_dup_key,
            i.is_primary_key,
            i.is_unique_constraint,
            i.fill_factor,
            i.is_padded,
            i.is_disabled,
            i.auto_created
        FROM sys.indexes                AS i
                 JOIN sys.index_columns AS ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
                 JOIN sys.objects       AS o ON i.object_id = o.object_id
        WHERE i.is_hypothetical = 0
          AND i.object_id = OBJECT_ID('Warehouse.StockItems');
      """;
}