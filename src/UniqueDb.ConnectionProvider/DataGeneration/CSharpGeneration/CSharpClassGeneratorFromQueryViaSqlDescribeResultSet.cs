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
            var sqlTypeList = GetSysTypesFromDatabase(sqlConnectionProvider, resultColumns);


            var describeResultSetContainers = new List<DescribeResultSetContainer>();
            foreach (var describeResultSetRow in resultColumns)
            {
                SqlSysType userDefinedType = null;
                SqlSysType systemType = null;
                if (describeResultSetRow.user_type_id.HasValue)
                {
                    userDefinedType =
                        sqlTypeList.Single(x => x.user_type_id == describeResultSetRow.user_type_id);

                    try
                    {
                        try
                        {
                            if (SqlTypes.IsSpecialSystemType(describeResultSetRow.user_type_name))
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

    public class DescribeResultSetContainer
    {
        public DescribeResultSetContainer(DescribeResultSetRow describeResultSetRow, SqlSysType systemType)
        {
            DescribeResultSetRow = describeResultSetRow;
            SystemType = systemType;
        }

        public DescribeResultSetContainer(DescribeResultSetRow describeResultSetRow, SqlSysType userDefinedType, SqlSysType systemType)
        {
            DescribeResultSetRow = describeResultSetRow;
            UserDefinedType = userDefinedType;
            SystemType = systemType;
        }

        public DescribeResultSetRow DescribeResultSetRow { get; set; }
        public SqlSysType UserDefinedType { get; set; }
        public SqlSysType SystemType { get; set; }
        
    }
}