namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpClassGeneratorHelpers
    {
        public static string GenerateClass(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery, string className)
        {
            var cSharpProperties = SqlQueryToCSharpPropertyGenerator.FromQuery(sqlConnectionProvider, sqlQuery);
            var cSharpClass = CSharpClassGenerator.GenerateClassText(className, cSharpProperties);
            return cSharpClass;
        }
    }
}