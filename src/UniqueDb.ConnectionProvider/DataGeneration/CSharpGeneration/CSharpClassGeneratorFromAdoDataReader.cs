namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration
{
    public static class CSharpClassGeneratorFromAdoDataReader
    {
        public static string GenerateClass(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery, string className)
        {
            var cSharpProperties = CSharpPropertyFactoryFromAdoDataReader.FromQuery(sqlConnectionProvider, sqlQuery);
            var cSharpClass = CSharpClassTextGenerator.GenerateClassText(className, cSharpProperties);
            return cSharpClass;
        }
    }
}