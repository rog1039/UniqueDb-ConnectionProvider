using UniqueDb.ConnectionProvider.Converters;
using UniqueDb.ConnectionProvider.CoreTypes;

namespace UniqueDb.ConnectionProvider.SqlMetadata.DescribeResultSet;

public static class DescribeResultSetRowToSqlColumnConverter
{
   public static SqlColumn Convert(DescribeResultSetContainer resultSetColumn)
   {
      var sqlType = GetSqlType(resultSetColumn);
      
      var sqlColumn = new SqlColumn
      {
         Name        = resultSetColumn.DescribeResultSetRow.name,
         IsNullable  = resultSetColumn.DescribeResultSetRow.is_nullable,
         IsIdentity  = resultSetColumn.DescribeResultSetRow.is_identity_column,
         IsComputed  = resultSetColumn.DescribeResultSetRow.is_computed_column,
         SqlDataType = sqlType
      };

      return sqlColumn;
   }

   private static SqlType GetSqlType(DescribeResultSetContainer resultSetColumn)
   {
      var ambiguousType = GetSqlTypeSpecification(resultSetColumn);
      var sqlType       = SqlTypeFromSpecification.Convert(ambiguousType);
      return sqlType;
   }

   private static SqlTypeSpecification GetSqlTypeSpecification(DescribeResultSetContainer resultSetContainer)
   {
      var describeResultSetRow = resultSetContainer.DescribeResultSetRow;
      var userType             = resultSetContainer.UserDefinedType;
      var systemType           = resultSetContainer.SystemType;
      var ambigiousType = new SqlTypeSpecification()
      {
         TypeName                   = systemType.name,
         NumericPrecision           = describeResultSetRow.precision,
         NumericScale               = describeResultSetRow.scale,
         MaxCharacterLength         = describeResultSetRow.max_length,
         FractionalSecondsPrecision = describeResultSetRow.scale
      };

      if (resultSetContainer.UserDefinedType != null)
      {
         ambigiousType.MaxCharacterText = userType.max_length == -1
            ? "max"
            : string.Empty;
      }

      return ambigiousType;
   }
}