namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlColumnFactory
    {
        public static SqlColumn Create(InformationSchemaColumn infSchColumn)
        {
            var sqlColumn = new SqlColumn()
            {
                Name = infSchColumn.COLUMN_NAME,
                SqlDataType = infSchColumn.DATA_TYPE,
                IsNullable = infSchColumn.IS_NULLABLE == "YES",
                OrdinalPosition = infSchColumn.ORDINAL_POSITION,
                Default = infSchColumn.COLUMN_DEFAULT
            };
            return sqlColumn;
        }
    }
}