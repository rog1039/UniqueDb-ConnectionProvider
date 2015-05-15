using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class CSharpPropertyFactoryFromDescribeResultSetRow
    {
        public static CSharpProperty ToCSharpProperty(DescribeResultSetRow row)
        {
            var property = new CSharpProperty();
            property.Name = row.name;
            property.ClrAccessModifier = ClrAccessModifier.Public;
            var sqlTypeParseResult = ParseSqlSystemTypeName(row);
            SetClrType(property, sqlTypeParseResult);
            property.IsNullable = row.is_nullable;

            property.DataAnnotationDefinitionBases.AddRange(GetDataAnnotations(row, sqlTypeParseResult));

            return property;
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

            public static SqlTypeParseResult Parse(string sqlSystemTypeName)
            {
                var sqlTypeParseResult = sqlSystemTypeName
                    .MakeList()
                    .MatchRegex(NoPrecisionMatchRegex, new NoPrecisionMatch())
                    .Select(SqlTypeParseResult.Create)
                    .SingleOrDefault();
                return sqlTypeParseResult;
            }
        }

        private class Precision1Match : NoPrecisionMatch
        {
            public string Precision1 { get; set; }
            public static SqlTypeParseResult Parse(string sqlSystemTypeName)
            {
                var sqlTypeParseResult = sqlSystemTypeName
                    .MakeList()
                    .MatchRegex(Precision1MatchRegex, new Precision1Match())
                    .Select(SqlTypeParseResult.Create)
                    .SingleOrDefault();
                return sqlTypeParseResult;
            }
        }

        private class Precision2Match : Precision1Match
        {
            public string Precision2 { get; set; }
            public static SqlTypeParseResult Parse(string sqlSystemTypeName)
            {
                var sqlTypeParseResult = sqlSystemTypeName
                    .MakeList()
                    .MatchRegex(Precision2MatchRegex, new Precision2Match())
                    .Select(SqlTypeParseResult.Create)
                    .SingleOrDefault();
                return sqlTypeParseResult;
            }
        }

        private static SqlTypeParseResult ParseSqlSystemTypeName(DescribeResultSetRow row)
        {
            if (string.IsNullOrEmpty(row.system_type_name))
            {
                return new SqlTypeParseResult() {SqlType = row.user_type_name, IsSystemDefined = false};
            }

            return Precision2Match.Parse(row.system_type_name) 
                ?? Precision1Match.Parse(row.system_type_name) 
                ?? NoPrecisionMatch.Parse(row.system_type_name);
        }

        private static void SetClrType(CSharpProperty property, SqlTypeParseResult sqlTypeParseResult)
        {
            if (sqlTypeParseResult.IsSystemDefined)
            {
                property.DataType = SqlTypeStringToClrTypeStringConverter.GetClrDataType(sqlTypeParseResult.SqlType);
            }
            else
            {
                property.DataType = sqlTypeParseResult.SqlType;
            }
        }

        private class SqlTypeParseResult
        {
            public string SqlType { get; set; }
            public int? Precision1 { get; set; }
            public int? Precision2 { get; set; }
            public bool IsSystemDefined { get; set; } = true;

            public static SqlTypeParseResult Create(Precision2Match match)
            {
                return new SqlTypeParseResult()
                {
                    SqlType = match.TypeName,
                    Precision1 = int.Parse(match.Precision1),
                    Precision2 = Int32.Parse(match.Precision2)
                };
            }

            public static SqlTypeParseResult Create(Precision1Match match)
            {
                return new SqlTypeParseResult()
                {
                    SqlType = match.TypeName,
                    Precision1 = int.Parse(match.Precision1),
                };
            }

            public static SqlTypeParseResult Create(NoPrecisionMatch match)
            {
                return new SqlTypeParseResult()
                {
                    SqlType = match.TypeName,
                };
            }
        }

        private static IEnumerable<DataAnnotationDefinitionBase> GetDataAnnotations(DescribeResultSetRow row, SqlTypeParseResult sqlTypeParseResult)
        {
            if (row.max_length > 0 && SqlTypes.IsCharType(row.system_type_name))
            {
                yield return new DataAnnotationDefinitionMaxCharacterLength(row.max_length);
            }
        }
    }
}