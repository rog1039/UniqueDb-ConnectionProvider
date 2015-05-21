using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

static internal class InformationSchemaColumnToSqlColumnConverter
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
        var sqlType = InformationSchemaColumnToSqlTypeConverter.Convert(column);
        return sqlType;
    }
}