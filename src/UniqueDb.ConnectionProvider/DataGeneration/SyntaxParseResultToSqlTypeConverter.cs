using System;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class SyntaxParseResultToSqlTypeConverter
    {
        public static SqlType Parse(string input)
        {
            var syntaxParseResult = SqlTypeSyntaxParser.ParseType(input);
            var sqlType = CreateSqlTypeFromSyntaxParseResult(syntaxParseResult);
            return sqlType;
        }

        private static SqlType CreateSqlTypeFromSyntaxParseResult(SyntaxParseResult syntaxParseResult)
        {
            var typeName = syntaxParseResult.SqlTypeName;
            if (typeName == "int")
            {
                return SqlTypeFactory.Int();
            }
            if (typeName == "smallint")
            {
                return SqlTypeFactory.SmallInt();
            }
            if (typeName == "decimal")
            {
                return SqlTypeFactory.Decimal(syntaxParseResult.Precision1.Value, syntaxParseResult.Precision2);
            }
            if (typeName == "numeric")
            {
                return SqlTypeFactory.Numeric(syntaxParseResult.Precision1.Value, syntaxParseResult.Precision2);
            }
            if (typeName == "float")
            {
                return SqlTypeFactory.Float(syntaxParseResult.Precision1.Value);
            }
            if (typeName == "real")
            {
                return SqlTypeFactory.Real();
            }
            if (typeName == "date")
            {
                return SqlTypeFactory.Date();
            }
            if (typeName == "datetime")
            {
                return SqlTypeFactory.DateTime();
            }
            if (typeName == "datetime2")
            {
                return SqlTypeFactory.DateTime2(syntaxParseResult.Precision1);
            }
            if (typeName == "char")
            {
                return SqlTypeFactory.Char(SqlTextualDataTypeOptionsFactory.Create(syntaxParseResult));
            }
            if (typeName == "varchar")
            {
                return SqlTypeFactory.VarChar(SqlTextualDataTypeOptionsFactory.Create(syntaxParseResult));
            }
            if (typeName == "nchar")
            {
                return SqlTypeFactory.NChar(SqlTextualDataTypeOptionsFactory.Create(syntaxParseResult));
            }
            if (typeName == "nvarchar")
            {
                return SqlTypeFactory.NVarChar(SqlTextualDataTypeOptionsFactory.Create(syntaxParseResult));
            }
            Console.WriteLine($"This is bad mkay: {typeName}");
            return new SqlType(typeName);
        }
    }

    public static class AmbigiousSqlTypeToSqlTypeConverter
    {
        public static SqlType Convert(AmbigiousSqlType ambigiousSqlType)
        {
            var typeName = ambigiousSqlType.TypeName;
            if (typeName == "int")
            {
                return SqlTypeFactory.Int();
            }
            if (typeName == "smallint")
            {
                return SqlTypeFactory.SmallInt();
            }
            if (typeName == "decimal")
            {
                return SqlTypeFactory.Decimal(ambigiousSqlType.NumericPrecision.Value, ambigiousSqlType.NumericScale);
            }
            if (typeName == "numeric")
            {
                return SqlTypeFactory.Numeric(ambigiousSqlType.NumericPrecision.Value, ambigiousSqlType.NumericScale);
            }
            if (typeName == "float")
            {
                return SqlTypeFactory.Float(ambigiousSqlType.NumericPrecision.Value);
            }
            if (typeName == "real")
            {
                return SqlTypeFactory.Real();
            }
            if (typeName == "date")
            {
                return SqlTypeFactory.Date();
            }
            if (typeName == "datetime")
            {
                return SqlTypeFactory.DateTime();
            }
            if (typeName == "datetime2")
            {
                return SqlTypeFactory.DateTime2(ambigiousSqlType.FractionalSecondsPrecision);
            }
            if (typeName == "char")
            {
                return SqlTypeFactory.Char(SqlTextualDataTypeOptionsFactory.FromAmbigiousSqlType(ambigiousSqlType));
            }
            if (typeName == "varchar")
            {
                return SqlTypeFactory.VarChar(SqlTextualDataTypeOptionsFactory.FromAmbigiousSqlType(ambigiousSqlType));
            }
            if (typeName == "nchar")
            {
                return SqlTypeFactory.NChar(SqlTextualDataTypeOptionsFactory.FromAmbigiousSqlType(ambigiousSqlType));
            }
            if (typeName == "nvarchar")
            {
                return SqlTypeFactory.NVarChar(SqlTextualDataTypeOptionsFactory.FromAmbigiousSqlType(ambigiousSqlType));
            }
            Console.WriteLine($"This is bad mkay: {typeName}");
            return new SqlType(typeName);
        }
    }

}