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
            var sqlDataType = SqlTypeParser.Parse(column.DATA_TYPE);
            sqlDataType.MaximumCharLength = column.CHARACTER_MAXIMUM_LENGTH;
            sqlDataType.NumericPrecision = column.NUMERIC_PRECISION;
            sqlDataType.NumericScale = column.NUMERIC_SCALE;
            sqlDataType.DateTimePrecision = column.DATETIME_PRECISION;
            return sqlDataType;
        }
    }
}