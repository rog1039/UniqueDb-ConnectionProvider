using System;
using UniqueDb.ConnectionProvider;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpClassGeneratorFromQueryViaAdo
    {
        public static string GenerateClass(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery, string className)
        {
            var cSharpProperties = CSharpPropertyFactoryFromFromSqlQuery.FromQuery(sqlConnectionProvider, sqlQuery);
            var cSharpClass = CSharpClassTextGenerator.GenerateClassText(className, cSharpProperties);
            return cSharpClass;
        }
    }
}