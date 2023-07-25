using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider.SqlMetadata.SysTables;

public class SysForeignKey
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

   private const string SqlQuery =
      """
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