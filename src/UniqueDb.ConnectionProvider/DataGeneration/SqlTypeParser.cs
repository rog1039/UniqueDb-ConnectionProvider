namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SqlTypeParser
    {
        public static SqlType Parse(string input)
        {
            var syntaxParseResult = ParseSyntax(input);

            var sqlType = CreateSqlTypeFromSyntaxParseResult(syntaxParseResult);

            return sqlType;
        }

        private static SyntaxParseResult ParseSyntax(string input)
        {
            var syntaxParseResult = SqlTypeSyntaxParser.ParseType(input);
            return syntaxParseResult;
        }

        private static SqlType CreateSqlTypeFromSyntaxParseResult(SyntaxParseResult syntaxParseResult)
        {
            var doesParseResultContainPrecision = syntaxParseResult.Precision1.HasValue;
            if (doesParseResultContainPrecision)
            {
                return CreateSqlTypeWithPrecision(syntaxParseResult);
            }
            else
            {
                return new SqlType()
                {
                    TypeName = syntaxParseResult.SqlTypeName,
                    IsSystemDefined = SqlTypes.IsSystemType(syntaxParseResult.SqlTypeName)
                };
            }
        }

        private static SqlType CreateSqlTypeWithPrecision(SyntaxParseResult syntaxParseResult)
        {
            var typeName = syntaxParseResult.SqlTypeName;
            if (typeName == "decimal" || typeName == "numeric")
            {
                return new SqlType(typeName, syntaxParseResult.Precision1, syntaxParseResult.Precision2);
            }
            if (typeName == "float" || typeName == "real")
            {
                return new SqlType(typeName, syntaxParseResult.Precision1);
            }
            if (typeName == "date" || typeName == "datetime")
            {
                return new SqlType(typeName);
            }
            if (typeName == "datetime2")
            {
                return SqlType.DateTime2(typeName, syntaxParseResult.Precision1);
            }
            if (typeName == "char" || typeName == "nchar" || typeName == "varchar" || typeName == "nvarchar")
            {
                return SqlType.TextType(typeName, syntaxParseResult.Precision1);
            }

            return new SqlType(typeName);
        }
    }
}