using System;
using System.Linq;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlResultSetColumnTypeParser
    {
        public static SqlResultSetColumnTypeParseResult ParseSqlSystemType(DescribeResultSetRow row)
        {
            if (String.IsNullOrEmpty(row.system_type_name))
            {
                return new SqlResultSetColumnTypeParseResult() {SqlType = row.user_type_name, IsSystemDefined = false};
            }

            return Precision2Match.Parse(row.system_type_name) 
                   ?? Precision1Match.Parse(row.system_type_name) 
                   ?? NoPrecisionMatch.Parse(row.system_type_name);
        }

        private const string Precision2MatchRegex =
            @"(?<TypeName>\w+)\((?<Precision1>\d+),(?<Precision2>\d+)\)";

        private const string Precision1MatchRegex =
            @"(?<TypeName>\w+)\((?<Precision1>\d+)\)";

        private const string NoPrecisionMatchRegex =
            @"(?<TypeName>\w+)";

        private class NoPrecisionMatch
        {
            public string TypeName { get; set; }

            public static SqlResultSetColumnTypeParseResult Parse(string sqlSystemTypeName)
            {
                var sqlTypeParseResult = Enumerable.SingleOrDefault<SqlResultSetColumnTypeParseResult>(sqlSystemTypeName
                    .MatchRegex(NoPrecisionMatchRegex, new NoPrecisionMatch())
                    .Select(Create));
                return sqlTypeParseResult;
            }
        }

        private class Precision1Match : NoPrecisionMatch
        {
            public string Precision1 { get; set; }
            public static SqlResultSetColumnTypeParseResult Parse(string sqlSystemTypeName)
            {
                var sqlTypeParseResult = Enumerable.SingleOrDefault<SqlResultSetColumnTypeParseResult>(sqlSystemTypeName
                    .MatchRegex(Precision1MatchRegex, new Precision1Match())
                    .Select(Create));
                return sqlTypeParseResult;
            }
        }

        private class Precision2Match : Precision1Match
        {
            public string Precision2 { get; set; }
            public static SqlResultSetColumnTypeParseResult Parse(string sqlSystemTypeName)
            {
                var sqlTypeParseResult = Enumerable.SingleOrDefault<SqlResultSetColumnTypeParseResult>(sqlSystemTypeName
                    .MatchRegex(Precision2MatchRegex, new Precision2Match())
                    .Select(Create));
                return sqlTypeParseResult;
            }
        }

        public class SqlResultSetColumnTypeParseResult
        {
            public string SqlType { get; set; }
            public int? Precision1 { get; set; }
            public int? Precision2 { get; set; }
            public bool IsSystemDefined { get; set; } = true;
        }
        
        private static SqlResultSetColumnTypeParseResult Create(Precision2Match match)
        {
            return new SqlResultSetColumnTypeParseResult()
            {
                SqlType = match.TypeName,
                Precision1 = Int32.Parse(match.Precision1),
                Precision2 = Int32.Parse(match.Precision2)
            };
        }

        private static SqlResultSetColumnTypeParseResult Create(Precision1Match match)
        {
            return new SqlResultSetColumnTypeParseResult()
            {
                SqlType = match.TypeName,
                Precision1 = Int32.Parse(match.Precision1),
            };
        }

        private static SqlResultSetColumnTypeParseResult Create(NoPrecisionMatch match)
        {
            return new SqlResultSetColumnTypeParseResult()
            {
                SqlType = match.TypeName,
            };
        }
    }
}