using System.Data.Common;

namespace UniqueDb.ConnectionProvider.CSharpGeneration;

public static class CSharpClassGeneratorFromAdoDataReader
{
    public static string GenerateClass(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery, string className, CSharpClassTextGeneratorOptions? options = null)
    {
        options ??= CSharpClassTextGeneratorOptions.Default;
        var cSharpProperties = CSharpPropertyFactoryFromAdoDataReader.FromQuery(sqlConnectionProvider, sqlQuery);
        var cSharpClass      = CSharpClassTextGenerator.GenerateClassText(className, cSharpProperties, options);
        return cSharpClass;
    }

    public static string GenerateClass(DbConnection connection, string sqlQuery, string className, CSharpClassTextGeneratorOptions? options = null)
    {
        options ??= CSharpClassTextGeneratorOptions.Default;
        var cSharpProperties = CSharpPropertyFactoryFromAdoDataReader.FromQuery(connection, sqlQuery);
        var cSharpClass      = CSharpClassTextGenerator.GenerateClassText(className, cSharpProperties, options);
        return cSharpClass;
    }
}