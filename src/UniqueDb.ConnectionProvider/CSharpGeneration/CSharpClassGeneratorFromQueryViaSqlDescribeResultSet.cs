using System.Data;
using Dapper;
using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata.DescribeResultSet;
using UniqueDb.ConnectionProvider.SqlMetadata.SysTables;

namespace UniqueDb.ConnectionProvider.CSharpGeneration;

public static class CSharpClassGeneratorFromQueryViaSqlDescribeResultSet
{
    public static string GenerateClass(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery, string className)
    {
        var cSharpProperties = GetCSharpProperties(sqlConnectionProvider, sqlQuery);
        var classText = CSharpClassTextGenerator.GenerateClassText(className, cSharpProperties, CSharpClassTextGeneratorOptions.Default);
        return classText.Trim();
    }

    private static List<CSharpProperty> GetCSharpProperties(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
    {
        var describeResultSetContainers = GetDescribeResultSetContainers(sqlConnectionProvider, sqlQuery);
        var cSharpProperties = describeResultSetContainers
            .Select(CSharpPropertyFactoryFromDescribeResultSetRow.ToCSharpProperty)
            .ToList();
        return cSharpProperties;
    }

    public static List<DescribeResultSetContainer> GetDescribeResultSetContainers(ISqlConnectionProvider sqlConnectionProvider, string sqlQuery)
    {
        var resultColumns = GetQueryResultColumns(sqlConnectionProvider, sqlQuery);
        var sqlTypeList   = GetSysTypesFromDatabase(sqlConnectionProvider, resultColumns);


        var describeResultSetContainers = new List<DescribeResultSetContainer>();
        foreach (var describeResultSetRow in resultColumns)
        {
            SqlSysType userDefinedType = null;
            SqlSysType systemType      = null;
            if (describeResultSetRow.user_type_id.HasValue)
            {
                userDefinedType =
                    sqlTypeList.Single(x => x.user_type_id == describeResultSetRow.user_type_id);

                try
                {
                    if (SqlTypeLists.IsSpecialSystemType(describeResultSetRow.user_type_name))
                    {
                        systemType = sqlTypeList
                            .Single(x => x.system_type_id == describeResultSetRow.system_type_id
                                      && x.user_type_id == describeResultSetRow.user_type_id);
                    }
                    else
                    {
                        systemType = sqlTypeList
                            .Single(x => x.system_type_id == describeResultSetRow.system_type_id
                                      && x.user_type_id == describeResultSetRow.system_type_id);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                systemType = sqlTypeList
                    .Single(x => x.system_type_id == describeResultSetRow.system_type_id
                              && x.user_type_id == describeResultSetRow.system_type_id);
            }

            var describeResultSetContainer = new DescribeResultSetContainer(
                describeResultSetRow,
                userDefinedType,
                systemType);
            describeResultSetContainers.Add(describeResultSetContainer);
        }
        return describeResultSetContainers;
    }

    private static IList<SqlSysType> GetSysTypesFromDatabase(ISqlConnectionProvider sqlConnectionProvider, IEnumerable<DescribeResultSetRow> resultColumns)
    {
        var sysTypes = sqlConnectionProvider
            .Query<SqlSysType>("select * from sys.types")
            .ToList();

        return sysTypes;
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