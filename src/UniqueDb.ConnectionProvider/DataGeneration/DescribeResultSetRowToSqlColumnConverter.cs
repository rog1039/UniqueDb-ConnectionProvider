using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class DescribeResultSetRowToSqlColumnConverter
    {
        public static SqlColumn Convert(DescribeResultSetRow resultSetColumn)
        {
            var sqlColumn = new SqlColumn();
            sqlColumn.Name = resultSetColumn.name;
            sqlColumn.IsNullable = resultSetColumn.is_nullable;

            var typeName = GetTypeName(resultSetColumn);
            sqlColumn.SqlDataType = SyntaxParseResultToSqlTypeConverter.Parse(typeName);
            return sqlColumn;
        }

        private static string GetTypeName(DescribeResultSetRow resultSetColumn)
        {
            var hasSystemType = resultSetColumn.system_type_name?.Length > 0;
            var hasUserType = resultSetColumn.user_type_name?.Length > 0;
            var noTypeSpecified = !hasSystemType && !hasUserType;
            var bothTypesSpecified = hasSystemType && hasUserType;
            var bothTypesEqual = String.Equals(resultSetColumn.system_type_name, resultSetColumn.user_type_name);

            if (noTypeSpecified || (bothTypesSpecified && !bothTypesEqual))
                throw new InvalidOperationException("Invalid SQL type specification.");

            var typeName = hasSystemType
                ? resultSetColumn.system_type_name
                : resultSetColumn.user_type_name;
            return typeName;
        }

        public static SqlColumn Convert(DescribeResultSetContainer resultSetColumn)
        {
            var sqlColumn = new SqlColumn();
            sqlColumn.Name = resultSetColumn.DescribeResultSetRow.name;
            sqlColumn.IsNullable = resultSetColumn.DescribeResultSetRow.is_nullable;

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
            if (resultSetContainer.UserDefinedType != null)
            {
                return new AmbigiousSqlType()
                {
                    TypeName = systemType.name,
                    NumericPrecision = describeResultSetRow.precision,
                    NumericScale = describeResultSetRow.scale,
                    MaxCharacterLength = describeResultSetRow.max_length,
                    FractionalSecondsPrecision = describeResultSetRow.scale,

                    MaxCharacterText = userType.max_length == -1
                        ? "max"
                        : string.Empty
                };
            }
            return new AmbigiousSqlType()
            {
                TypeName = systemType.name,
                NumericPrecision = describeResultSetRow.precision,
                NumericScale = describeResultSetRow.scale,
                MaxCharacterLength = describeResultSetRow.max_length,
                FractionalSecondsPrecision = describeResultSetRow.precision
            };
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