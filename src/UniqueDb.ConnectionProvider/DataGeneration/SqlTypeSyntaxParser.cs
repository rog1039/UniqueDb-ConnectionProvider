using System;
using System.Linq;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlTypeSyntaxParser
    {
        public static SyntaxParseResult ParseSqlSystemType(DescribeResultSetRow row)
        {
            if (string.IsNullOrEmpty(row.system_type_name))
            {
                return ParseType(row.user_type_name);
            }

            return ParseType(row.system_type_name);
        }

        public static SyntaxParseResult ParseType(string sqlType)
        {
            return Precision2Match.Parse(sqlType)
                   ?? Precision1Match.Parse(sqlType)
                   ?? NoPrecisionMatch.Parse(sqlType);
        }

        private class NoPrecisionMatch
        {
            public string Input { get; set; }
            public string TypeName { get; set; }

            private const string NoPrecisionMatchRegex =
                @"(?<TypeName>\w+)";

            public static SyntaxParseResult Parse(string sqlSystemTypeName)
            {
                var sqlTypeParseResult = sqlSystemTypeName
                    .MatchRegex(NoPrecisionMatchRegex, new NoPrecisionMatch())
                    .Select(Create)
                    .SingleOrDefault();
                return sqlTypeParseResult;
            }
        }

        private class Precision1Match : NoPrecisionMatch
        {
            public string Precision1 { get; set; }

            private const string Precision1MatchRegex =
                @"(?<TypeName>\w+)\((?<Precision1>\d+)\)";

            public static SyntaxParseResult Parse(string sqlSystemTypeName)
            {
                var sqlTypeParseResult = sqlSystemTypeName
                    .MatchRegex(Precision1MatchRegex, new Precision1Match())
                    .Select(Create)
                    .SingleOrDefault();
                return sqlTypeParseResult;
            }
        }

        private class Precision2Match : Precision1Match
        {
            public string Precision2 { get; set; }

            private const string Precision2MatchRegex =
                @"(?<TypeName>\w+)\((?<Precision1>\d+),(?<Precision2>\d+)\)";


            public static SyntaxParseResult Parse(string sqlSystemTypeName)
            {
                var sqlTypeParseResult = sqlSystemTypeName
                    .MatchRegex(Precision2MatchRegex, new Precision2Match())
                    .Select(Create)
                    .SingleOrDefault();
                return sqlTypeParseResult;
            }
        }

        private static SyntaxParseResult Create(Precision2Match match)
        {
            return new SyntaxParseResult(
                match.TypeName,
                Int32.Parse(match.Precision1),
                Int32.Parse(match.Precision2),
                match.Input);
        }

        private static SyntaxParseResult Create(Precision1Match match)
        {
            return new SyntaxParseResult(
                match.TypeName,
                Int32.Parse(match.Precision1),
                match.Input);
        }

        private static SyntaxParseResult Create(NoPrecisionMatch match)
        {
            return new SyntaxParseResult(
                match.TypeName,
                match.Input);
        }
    }
}