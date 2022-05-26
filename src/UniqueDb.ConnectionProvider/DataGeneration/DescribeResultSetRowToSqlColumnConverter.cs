using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration;

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
      var ambiguousType = GetAmbiguousSqlType(resultSetColumn);
      var sqlType       = AmbiguousSqlTypeToSqlTypeConverter.Convert(ambiguousType);
      return sqlType;
   }

   private static AmbiguousSqlType GetAmbiguousSqlType(DescribeResultSetContainer resultSetContainer)
   {
      var describeResultSetRow = resultSetContainer.DescribeResultSetRow;
      var userType             = resultSetContainer.UserDefinedType;
      var systemType           = resultSetContainer.SystemType;
      var ambigiousType = new AmbiguousSqlType()
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

/// <summary>
/// Just a copy of values from the DescribeResultSet. Further analysis can narrow this type into
/// the more defined SqlType.
/// </summary>
public class AmbiguousSqlType
{
   public string TypeName                   { get; set; }
   public int?   NumericPrecision           { get; set; }
   public int?   NumericScale               { get; set; }
   public int?   MaxCharacterLength         { get; set; }
   public string MaxCharacterText           { get; set; }
   public int?   FractionalSecondsPrecision { get; set; }
}