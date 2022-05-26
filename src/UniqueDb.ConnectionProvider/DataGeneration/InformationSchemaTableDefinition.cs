using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public class InformationSchemaTableDefinition
{
   public int Id { get; set; }

   public SISTable                      InformationSchemaTable   { get; set; }
   public IList<SISColumn>              InformationSchemaColumns { get; set; }
   public IList<TableConstraintInfoDto> TableConstraints         { get; set; }

   public IList<SISColumn> GetPrimaryKeyColumns()
   {
      return InformationSchemaColumns
         .Where(IsColumnPrimaryKey)
         .ToList();
   }


   public bool IsColumnPrimaryKey(SISColumn col)
   {
      if (col.TABLE_SCHEMA != InformationSchemaTable.TABLE_SCHEMA ||
          col.TABLE_NAME != InformationSchemaTable.TABLE_NAME)
      {
         var columnTableName = $"{col.TABLE_SCHEMA}.{col.TABLE_NAME}";
         var tableTableName  = $"{InformationSchemaTable.TABLE_SCHEMA}.{InformationSchemaTable.TABLE_NAME}";

         throw new Exception(
            $"Column and Table names do not match. Col: {columnTableName}; Table:{tableTableName}");
      }

      var columnName = col.COLUMN_NAME;
      return IsColumnPrimaryKey(columnName);
   }

   public bool IsColumnPrimaryKey(string columnName)
   {
      var isPrimaryKey = TableConstraints
         .Where(z => z.CONSTRAINT_TYPE == TableConstraintInfoDto.PrimaryKeyConstraintType)
         .Any(z => z.COLUMN_NAME.InsensitiveEquals(columnName));
      return isPrimaryKey;
   }
}