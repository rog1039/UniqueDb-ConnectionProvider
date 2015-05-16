using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpClassGeneratorFromQueryViaSqlDescribeResultSet
    {
        public static string GenerateClass(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery, string className)
        {
            var cSharpProperties = GetCSharpProperties(sqlConnectionProvider, sqlQuery);
            var classText = CSharpClassTextGenerator.GenerateClassText(className, cSharpProperties);
            return classText;
        }

        private static List<CSharpProperty> GetCSharpProperties(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
        {
            var resultColumns = GetQueryResultColumns(sqlConnectionProvider, sqlQuery);
            var cSharpProperties = resultColumns
                .Select(CSharpPropertyFactoryFromDescribeResultSetRow.ToCSharpProperty)
                .ToList();
            return cSharpProperties;
        }

        private static IEnumerable<DescribeResultSetRow> GetQueryResultColumns(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@tsql", sqlQuery);

            var resultSetColumns = sqlConnectionProvider
                .GetSqlConnection()
                .Query<DescribeResultSetRow>(DescribeResultSetStoredProcedureName, parameter, null, true, null, CommandType.StoredProcedure);
            return resultSetColumns;
        }

        private const string DescribeResultSetStoredProcedureName = "sp_describe_first_result_set";
    }
}