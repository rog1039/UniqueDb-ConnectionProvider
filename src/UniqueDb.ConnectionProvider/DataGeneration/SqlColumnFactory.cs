using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlColumnFactory
    {
        public static SqlColumn FromInformationSchemaColumn(InformationSchemaColumn column)
        {
            var sqlColumn = new SqlColumn()
            {
                Name = column.COLUMN_NAME,
                IsNullable = column.IS_NULLABLE == "YES",
                OrdinalPosition = column.ORDINAL_POSITION,
                Default = column.COLUMN_DEFAULT,
            };

            sqlColumn.SqlDataType = GetSqlTypeFromInformationSchemaColumn(column);
            return sqlColumn;
        }

        private static SqlType GetSqlTypeFromInformationSchemaColumn(InformationSchemaColumn column)
        {
            var syntaxParseResult = new SyntaxParseResult(column.DATA_TYPE, string.Empty);

            var sqlDataType = SyntaxParseResultToSqlTypeConverter.Parse(column.DATA_TYPE);
            sqlDataType.MaximumCharLength = column.CHARACTER_MAXIMUM_LENGTH;
            sqlDataType.NumericPrecision = column.NUMERIC_PRECISION;
            sqlDataType.NumericScale = column.NUMERIC_SCALE;
            sqlDataType.FractionalSecondsPrecision = column.DATETIME_PRECISION;
            return sqlDataType;
        }

        

        public static SqlColumn FromDescribeResultSetRow(DescribeResultSetRow resultSetColumn)
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
            var hasSystemType =  resultSetColumn.system_type_name?.Length > 0;
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
    }
}