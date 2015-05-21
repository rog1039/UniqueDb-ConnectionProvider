using System.Linq;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration
{
    public static class CSharpClassGeneratorFromInformationSchema
    {
        public static string CreateCSharpClass(SqlTableReference sqlTableReference, string className = default(string))
        {
            var schemaColumns = InformationSchemaMetadataExplorer.GetInformationSchemaColumns(sqlTableReference);
            var sqlColumns = schemaColumns.Select(x => InformationSchemaColumnToSqlColumn(x)).ToList();
            var cSharpProperties = sqlColumns.Select(x => CSharpPropertyFactoryFromSqlColumn.ToCSharpProperty(x)).ToList();

            var tableName = className ?? sqlTableReference.TableName;
            var classText = CSharpClassTextGenerator.GenerateClassText(tableName, cSharpProperties);
            return classText.Trim();
        }

        public static SqlColumn InformationSchemaColumnToSqlColumn(InformationSchemaColumn column)
        {
            var sqlColumn = new SqlColumn()
            {
                Name = column.COLUMN_NAME,
                IsNullable = column.IS_NULLABLE == "YES",
                OrdinalPosition = column.ORDINAL_POSITION,
                Default = column.COLUMN_DEFAULT,
            };

            var ambigiousSqlType = CSharpClassGeneratorFromInformationSchema.FromInformationSchemaColumn(column);
            sqlColumn.SqlDataType = AmbigiousSqlTypeToSqlTypeConverter.Convert(ambigiousSqlType);
            return sqlColumn;
        }

        public static AmbigiousSqlType FromInformationSchemaColumn(InformationSchemaColumn column)
        {
            var ambigiousSqlType = new AmbigiousSqlType()
            {
                TypeName = column.DATA_TYPE,
                NumericPrecision = column.NUMERIC_PRECISION,
                NumericScale = column.NUMERIC_SCALE,
                FractionalSecondsPrecision = column.DATETIME_PRECISION,
                MaxCharacterLength = column.CHARACTER_OCTET_LENGTH,
            };
            return ambigiousSqlType;
        }
    }
}