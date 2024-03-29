using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.SqlMetadata.InformationSchema;

namespace UniqueDb.ConnectionProvider.Converters;

public static class SqlTypeFromSISColumn
{
   public static SqlType FromInformationSchemaColumn(SISColumn col)
   {
      if (SqlTypeLists.IsDateTime(col.DATA_TYPE))
      {
         return SqlType.DateTime(col.DATA_TYPE, col.DATETIME_PRECISION);
      }
      if (SqlTypeLists.IsApproximateNumeric(col.DATA_TYPE))
      {
         if (col.DATA_TYPE == "float") return SqlTypeFactory.Float(col.NUMERIC_PRECISION.Value);
         if (col.DATA_TYPE == "real") return SqlTypeFactory.Real();
      }
      if (SqlTypeLists.IsExactNumeric(col.DATA_TYPE))
      {
         return SqlType.ExactNumericType(col.DATA_TYPE, col.NUMERIC_PRECISION, col.NUMERIC_SCALE);
      }
      if (SqlTypeLists.IsCharType(col.DATA_TYPE))
      {
         return SqlType.TextType(col.DATA_TYPE, col.CHARACTER_MAXIMUM_LENGTH);
      }

      if (col.DATA_TYPE.Equals("uniqueidentifier", StringComparison.InvariantCultureIgnoreCase))
      {
         return SqlType.Type(col.DATA_TYPE);
      }
      if (col.DATA_TYPE.Equals("varbinary", StringComparison.InvariantCultureIgnoreCase))
      {
         return SqlType.TextType(col.DATA_TYPE, col.CHARACTER_MAXIMUM_LENGTH);
      }
      throw new NotImplementedException($"Unknown type {col.DATA_TYPE}");
   }
}