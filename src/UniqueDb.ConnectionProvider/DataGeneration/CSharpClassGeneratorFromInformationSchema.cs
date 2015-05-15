using System.Linq;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpClassGeneratorFromInformationSchema
    {
        public static string GenerateClass()
        {
            return string.Empty;
        }

        public static string CreateCSharpClass(SqlTableReference sqlTableReference, string className = default(string))
        {
            var schemaColumns = InformationSchemaMetadataExplorer.GetInformationSchemaColumns(sqlTableReference);
            var cSharpProperties = schemaColumns.Select(CSharpPropertyFactoryFromInformationSchemaColumn.ToCSharpProperty).ToList();
            var tableName = className ?? sqlTableReference.TableName;
            var classText = CSharpClassTextGenerator.GenerateClassText(tableName, cSharpProperties);
            return classText;
        }
    }
}