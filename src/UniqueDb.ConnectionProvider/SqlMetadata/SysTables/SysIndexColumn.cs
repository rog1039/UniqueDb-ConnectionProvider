using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider.SqlMetadata.SysTables;

public class SysIndexColumn
{
   public int     index_id             { get; set; }
   public int     column_id            { get; set; }
   public int     index_column_id      { get; set; }
   public string? Index_Name           { get; set; }
   public string? TableName            { get; set; }
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
      SELECT b.index_id,
             b.column_id,
             b.index_column_id,
             a.name                             AS Index_Name,
             OBJECT_NAME(a.object_id)           as TableName,
             COL_NAME(b.object_id, b.column_id) AS Column_Name,
             b.is_descending_key,
             b.key_ordinal,
             b.is_included_column,
             a.type IndexType,
             a.type_desc IndexTypeDesc,
             a.is_unique,
             a.data_space_id,
             a.ignore_dup_key,
             a.is_primary_key,
             a.is_unique_constraint,
             a.fill_factor,
             a.is_padded,
             a.is_disabled,
             a.auto_created
      FROM
       sys.indexes AS a
      INNER JOIN
       sys.index_columns AS b
             ON a.object_id = b.object_id AND a.index_id = b.index_id
      WHERE
              a.is_hypothetical = 0 AND
       a.object_id = OBJECT_ID('$schema$.[$tableName$]');
      """;
}