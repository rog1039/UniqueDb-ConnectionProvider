using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class DescribeResultSetRowToSqlColumnConverter
    {
        public static SqlColumn Convert(DescribeResultSetContainer resultSetColumn)
        {
            var sqlColumn = new SqlColumn();
            sqlColumn.Name = resultSetColumn.DescribeResultSetRow.name;
            sqlColumn.IsNullable = resultSetColumn.DescribeResultSetRow.is_nullable;
            sqlColumn.IsIdentity = resultSetColumn.DescribeResultSetRow.is_identity_column;
            sqlColumn.IsComputed = resultSetColumn.DescribeResultSetRow.is_computed_column;

            SqlType sqlType = null;
            var isUserDefinedType = resultSetColumn.UserDefinedType != null;
            if (isUserDefinedType)
            {
                var ambigiousSqlType = GetAmbigiousSqlType(resultSetColumn);
                sqlType = AmbigiousSqlTypeToSqlTypeConverter.Convert(ambigiousSqlType);
            }
            else
            {
                sqlType = AmbigiousSqlTypeToSqlTypeConverter.Convert(
                    new AmbigiousSqlType()
                    {
                        TypeName = resultSetColumn.SystemType.name,
                        NumericPrecision = resultSetColumn.DescribeResultSetRow.precision,
                        NumericScale = resultSetColumn.DescribeResultSetRow.scale,
                        MaxCharacterLength = resultSetColumn.DescribeResultSetRow.max_length,
                        FractionalSecondsPrecision = resultSetColumn.DescribeResultSetRow.scale
                    });
            }
            sqlColumn.SqlDataType = sqlType;
            
            return sqlColumn;
        }

        private static AmbigiousSqlType GetAmbigiousSqlType(DescribeResultSetContainer resultSetContainer)
        {
            var describeResultSetRow = resultSetContainer.DescribeResultSetRow;
            var userType = resultSetContainer.UserDefinedType;
            var systemType = resultSetContainer.SystemType;
            var ambigiousType = new AmbigiousSqlType()
            {
                TypeName = systemType.name,
                NumericPrecision = describeResultSetRow.precision,
                NumericScale = describeResultSetRow.scale,
                MaxCharacterLength = describeResultSetRow.max_length,
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

    public class AmbigiousSqlType
    {
        public string TypeName { get; set; }
        public int? NumericPrecision { get; set; }
        public int? NumericScale { get; set; }
        public int? MaxCharacterLength { get; set; }
        public string MaxCharacterText { get; set; }
        public int? FractionalSecondsPrecision { get; set; }
    }
}