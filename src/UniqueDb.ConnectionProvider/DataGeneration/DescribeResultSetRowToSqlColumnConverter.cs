using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class DescribeResultSetRowToSqlColumnConverter
    {
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
                FractionalSecondsPrecision = describeResultSetRow.scale
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