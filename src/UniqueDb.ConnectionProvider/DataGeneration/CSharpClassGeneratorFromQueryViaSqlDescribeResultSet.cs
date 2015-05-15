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
            var resultSetColumns = GetDescribeResultSetRows(sqlConnectionProvider, sqlQuery);
            resultSetColumns.PrintStringTable();
            var cSharpProperties = resultSetColumns
                .Select(CSharpPropertyFactoryFromDescribeResultSetRow.ToCSharpProperty)
                .ToList();
            cSharpProperties.PrintStringTable();
            return cSharpProperties;
        }

        private static IEnumerable<DescribeResultSetRow> GetDescribeResultSetRows(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
        {
            var storedProcQuery = $"sp_describe_first_result_set @tsql = N'{sqlQuery}'";
            var parameter = new DynamicParameters();
            parameter.Add("@tsql", sqlQuery);
            Console.WriteLine(sqlQuery);
            Console.WriteLine(storedProcQuery);


            

            var resultSetColumns = sqlConnectionProvider
                .GetSqlConnection()
                .Query<DescribeResultSetRow>(DescribeResultSetStoredProcedureName, parameter, null, true, null, CommandType.StoredProcedure);
            return resultSetColumns;
        }

        private const string DescribeResultSetStoredProcedureName = "sp_describe_first_result_set";
    }
}