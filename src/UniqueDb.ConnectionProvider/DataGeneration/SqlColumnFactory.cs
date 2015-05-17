using System;
using System.Data;
using System.Diagnostics;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlColumnFactory
    {
        public static SqlColumn FromSqlServerMetadata(object columnMetadata)
        {
            throw new NotImplementedException();
        }

        public static SqlColumn FromInformationSchemaColumn(InformationSchemaColumn column)
        {
            var sqlColumn = new SqlColumn()
            {
                Name = column.COLUMN_NAME,
                SqlDataType = SqlTypeParser.Parse(column.DATA_TYPE),
                IsNullable = column.IS_NULLABLE == "YES",
                OrdinalPosition = column.ORDINAL_POSITION,
                Default = column.COLUMN_DEFAULT,
                CharacterMaxLength = column.CHARACTER_MAXIMUM_LENGTH
            };
            return sqlColumn;
        }

        public static SqlColumn FromAdoSchemaTable(DataRow row)
        {
            throw new NotImplementedException();
        }
    }
}