namespace UniqueDb.ConnectionProvider.SqlMetadata.InformationSchema;

public class SISTableConstraint
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