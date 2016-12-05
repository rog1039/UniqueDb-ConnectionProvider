using System.Data.Common;
using System.Data.Odbc;

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

        public static string GenerateClass(DbConnection connection, string sqlQuery, string className)
        {
            var cSharpProperties = CSharpPropertyFactoryFromAdoDataReader.FromQuery(connection, sqlQuery);
            var cSharpClass = CSharpClassTextGenerator.GenerateClassText(className, cSharpProperties);
            return cSharpClass;
        }
    }
}