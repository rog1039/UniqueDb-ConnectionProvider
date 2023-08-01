using UniqueDb.ConnectionProvider.CoreTypes;

namespace UniqueDb.ConnectionProvider.CSharpGeneration;

public static class CSharpClassGeneratorFromSqlTable
{
    public static string GenerateClass(SqlTable table, string className = null)
    {
        var cSharpProperties = table.SqlColumns
            .Select(CSharpPropertyFactoryFromSqlColumn.ToCSharpProperty)
            .ToList();
        className = className ?? table.Name;
        var cSharpClass = CSharpClassTextGenerator.GenerateClassText(className, cSharpProperties, CSharpClassTextGeneratorOptions.Default);
        return cSharpClass;
    }
}