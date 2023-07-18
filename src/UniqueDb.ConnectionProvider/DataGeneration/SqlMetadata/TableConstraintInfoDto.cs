using Woeber.Logistics.FluentDbMigrations.Tests;

namespace UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

public class TableConstraintInfoDto
{
   public int Id          { get; set; }
   public int SchemaDefId { get; set; }

   public string TABLE_CATALOG      { get; set; }
   public string TABLE_SCHEMA       { get; set; }
   public string TABLE_NAME         { get; set; }
   public string CONSTRAINT_TYPE    { get; set; }
   public string IS_DEFERRABLE      { get; set; }
   public string INITIALLY_DEFERRED { get; set; }
   public string CONSTRAINT_NAME    { get; set; }
   public string COLUMN_NAME        { get; set; }
   public int    ORDINAL_POSITION   { get; set; }
   public string DATA_TYPE          { get; set; }

   public const string PrimaryKeyConstraintType = @"PRIMARY KEY";

   public const string SqlQuery = @"
SELECT
    --tableConstraint.CONSTRAINT_CATALOG,
    --tableConstraint.CONSTRAINT_SCHEMA,
    --tableConstraint.CONSTRAINT_NAME,
    tableConstraint.TABLE_CATALOG,
    tableConstraint.TABLE_SCHEMA,
    tableConstraint.TABLE_NAME,
    tableConstraint.CONSTRAINT_TYPE,
    tableConstraint.IS_DEFERRABLE,
    tableConstraint.INITIALLY_DEFERRED,
    --keyColumn.CONSTRAINT_CATALOG,
    --keyColumn.CONSTRAINT_SCHEMA,
    keyColumn.CONSTRAINT_NAME,
    --keyColumn.TABLE_CATALOG,
    --keyColumn.TABLE_SCHEMA,
    --keyColumn.TABLE_NAME,
    keyColumn.COLUMN_NAME,
    keyColumn.ORDINAL_POSITION,
    col.DATA_TYPE
FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS tableConstraint
    JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE keyColumn
        ON keyColumn.CONSTRAINT_CATALOG = tableConstraint.CONSTRAINT_CATALOG
           AND keyColumn.CONSTRAINT_SCHEMA = tableConstraint.CONSTRAINT_SCHEMA
           AND keyColumn.CONSTRAINT_NAME = tableConstraint.CONSTRAINT_NAME
    JOIN INFORMATION_SCHEMA.COLUMNS col
        ON col.TABLE_CATALOG = keyColumn.TABLE_CATALOG
           AND col.TABLE_SCHEMA = keyColumn.TABLE_SCHEMA
           AND col.TABLE_NAME = keyColumn.TABLE_NAME
           AND col.COLUMN_NAME = keyColumn.COLUMN_NAME
--WHERE
ORDER BY keyColumn.CONSTRAINT_CATALOG,
         keyColumn.CONSTRAINT_SCHEMA,
         tableConstraint.TABLE_NAME,
         tableConstraint.CONSTRAINT_NAME,
         keyColumn.ORDINAL_POSITION;";
}


public class IndexColumnQueryDto
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

      public const string SqlQuery = $"""
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

public class FkConstraintColumnDto
{
   public string? FkSchema           { get; set; }
   public string  FkTable            { get; set; }
   public string? PkSchema           { get; set; }
   public string  PkTable            { get; set; }
   public int     ColumnNumber       { get; set; }
   public string? FkColumnName       { get; set; }
   public string? PkColumnName       { get; set; }
   public string  FkConstraintName   { get; set; }
   public string? FkTypeDesc         { get; set; }
   public string? FkTypeNum          { get; set; }
   public byte?   DeleteRefActionNum { get; set; }
   public string? DeleteRefAction    { get; set; }
   public bool    MsShipped          { get; set; }
   public bool    SystemNamed        { get; set; }
   public byte?   UpdateRefActionNum { get; set; }
   public string? UpdateRefAction    { get; set; }

   public static string GetSqlQuery(SqlTableReference reference)
   {
      var query = SqlQuery
            .MyReplace("fkSchemaName", reference.SchemaName)
            .MyReplace("fkTableName",  reference.TableName)
         ;
      return query;
   }
   
   private const string SqlQuery = $"""
SELECT SCHEMA_NAME(fk_tab.schema_id)     AS FkSchema,
       fk_tab.name                       AS FkTable,
       SCHEMA_NAME(pk_tab.schema_id)     AS PkSchema,
       pk_tab.name                       AS PkTable,
       fk_cols.constraint_column_id      AS ColumnNumber,
       fk_col.name                       AS FkColumnName,
       pk_col.name                       AS PkColumnName,
       fk.name                           AS FkConstraintName,
       fk.type_desc                      AS FkTypeDesc,
       fk.type                           AS FkTypeNum,
       fk.delete_referential_action      AS DeleteRefActionNum,
       fk.delete_referential_action_desc AS DeleteRefAction,
       fk.is_ms_shipped                  AS MsShipped,
       fk.is_system_named                AS SystemNamed,
       fk.update_referential_action      AS UpdateRefActionNum,
       fk.update_referential_action_desc AS UpdateRefAction
FROM sys.foreign_keys fk
         INNER JOIN sys.tables fk_tab
                    ON fk_tab.object_id = fk.parent_object_id
         INNER JOIN sys.tables pk_tab
                    ON pk_tab.object_id = fk.referenced_object_id
         INNER JOIN sys.foreign_key_columns fk_cols
                    ON fk_cols.constraint_object_id = fk.object_id
         INNER JOIN sys.columns fk_col
                    ON fk_col.column_id = fk_cols.parent_column_id
                        AND fk_col.object_id = fk_tab.object_id
         INNER JOIN sys.columns pk_col
                    ON pk_col.column_id = fk_cols.referenced_column_id
                        AND pk_col.object_id = pk_tab.object_id

WHERE SCHEMA_NAME(fk_tab.schema_id) = '$fkSchemaName$' AND fk_tab.name = '$fkTableName$'

ORDER BY SCHEMA_NAME(fk_tab.schema_id) + '.' + fk_tab.name,
         SCHEMA_NAME(pk_tab.schema_id) + '.' + pk_tab.name,
         fk_cols.constraint_column_id
""";
}