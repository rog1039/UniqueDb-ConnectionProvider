using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;

public static class CSharpClassGeneratorFromInformationSchema
{
    public static string CreateCSharpClass(SqlTableReference sqlTableReference, string? className = default(string))
    {
        var tableName = className ?? sqlTableReference.TableName;
        var schemaColumns    = InformationSchemaMetadataExplorer.GetInformationSchemaColumns(sqlTableReference);
        
        return CreateCSharpClass(schemaColumns, tableName, CSharpClassTextGeneratorOptions.Default);
    }

    public static string CreateCSharpClass(IEnumerable<SISColumn> schemaColumns, 
                                           string tableName,
                                           CSharpClassTextGeneratorOptions generatorOptions)
    {
        generatorOptions ??= CSharpClassTextGeneratorOptions.Default;
        
        var sqlColumns       = schemaColumns.Select(InformationSchemaColumnToSqlColumn).ToList();
        var cSharpProperties = sqlColumns.Select(CSharpPropertyFactoryFromSqlColumn.ToCSharpProperty).ToList();

        var classText =
            CSharpClassTextGenerator.GenerateClassText(tableName, 
                cSharpProperties, 
                generatorOptions);
        return classText.Trim();
    }

    public static SqlColumn InformationSchemaColumnToSqlColumn(SISColumn column)
    {
        var sqlColumn = new SqlColumn()
        {
            Name            = column.COLUMN_NAME,
            IsNullable      = column.IS_NULLABLE == "YES",
            OrdinalPosition = column.ORDINAL_POSITION,
            Default         = column.COLUMN_DEFAULT,
        };

        var ambigiousSqlType = FromInformationSchemaColumn(column);
        sqlColumn.SqlDataType = AmbiguousSqlTypeToSqlTypeConverter.Convert(ambigiousSqlType);
        return sqlColumn;
    }

    private static AmbiguousSqlType FromInformationSchemaColumn(SISColumn column)
    {
        var ambigiousSqlType = new AmbiguousSqlType()
        {
            TypeName                   = column.DATA_TYPE,
            NumericPrecision           = column.NUMERIC_PRECISION,
            NumericScale               = column.NUMERIC_SCALE,
            FractionalSecondsPrecision = column.DATETIME_PRECISION,
            MaxCharacterLength         = column.CHARACTER_OCTET_LENGTH,
        };
        return ambigiousSqlType;
    }
}