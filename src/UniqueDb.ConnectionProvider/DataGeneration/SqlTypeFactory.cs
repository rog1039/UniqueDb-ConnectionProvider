using System;
using System.Diagnostics.Contracts;
using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class SqlTypeFactory
    {
        private const long bigIntLowerBound = -9223372036854775808;
        private const long bigIntUpperBound = 9223372036854775807;

        /****   Exact Numerics   ****/
        public static SqlType Bit()
        {
            return SqlType.Type("bit");
        }
        public static SqlTypeNumberBase TinyInt()
        {
            return SqlTypeNumberBase.FromBounds("tinyint", 0, 255);
        }
        public static SqlTypeNumberBase SmallInt()
        {
            return SqlTypeNumberBase.FromBounds("smallint", -32768, 32767);
        }
        public static SqlTypeNumberBase Int()
        {
            return SqlTypeNumberBase.FromBounds("int", int.MinValue, int.MaxValue);
        }
        public static SqlTypeNumberBase BigInt()
        {
            return SqlTypeNumberBase.FromBounds("bigint", bigIntLowerBound, bigIntUpperBound);
        }
        public static SqlTypeNumberBase Decimal(int precision, int? scale = 0)
        {
            Contract.Requires(precision >= 1 && precision <= 38);
            Contract.Requires(!scale.HasValue || (scale >= 0 && scale <= precision));

            return SqlTypeNumberBase.FromPrecisionAndScale("decimal", precision, scale);
        }
        public static SqlTypeNumberBase Numeric(int precision, int? scale)
        {
            return Decimal(precision, scale);
        }
        public static SqlTypeNumberBase Money()
        {
            return SqlTypeNumberBase.FromBounds("money", -922337203685477.5808, 922337203685477.5807);
        }
        public static SqlTypeNumberBase SmallMoney()
        {
            return  SqlTypeNumberBase.FromBounds("smallmoney", -214748.3648, 214748.3647);
        }

        
        /****   Approximate Numerics   ****/
        public static SqlType Float(int numberOfBits = 53)
        {
            Contract.Requires(numberOfBits <= 53 && numberOfBits > 0);
            return SqlType.ApproximateNumeric("float", numberOfBits);
        }
        public static SqlType Real()
        {
            return SqlType.ApproximateNumeric("real", 24);
        }

        
        /****   Date & Time   ****/
        public static SqlType SmallDateTime()
        {
            return SqlType.Type("smalldatetime");
        }
        public static SqlType Date()
        {
            return SqlType.Type("date");
        }
        public static SqlType DateTime()
        {
            return SqlType.Type("datetime");
        }
        public static SqlType DateTime2(int? fractionalSecondsPrecision)
        {
            Contract.Requires(fractionalSecondsPrecision >= 0 && fractionalSecondsPrecision <= 7);
            return SqlType.DateTime("datetime2", fractionalSecondsPrecision);
        }
        public static SqlType DateTimeOffset()
        {
            return SqlType.Type("datetimeoffset");
        }
        public static SqlType Time()
        {
            return SqlType.Type("time");
        }

        
        /****   Text   ****/
        public static SqlType Char(SqlTextualDataTypeOptions options)
        {
            return SqlType.TextType("char", options.CharacterLength);
        }
        public static SqlType VarChar(SqlTextualDataTypeOptions options)
        {
            return SqlType.TextType("varchar", options.CharacterLength);
        }
        public static SqlType NChar(SqlTextualDataTypeOptions options)
        {
            return SqlType.TextType("nchar", options.CharacterLength);
        }
        public static SqlType NVarChar(SqlTextualDataTypeOptions options)
        {
            return SqlType.TextType("nvarchar", options.CharacterLength);
        }
        public static SqlType Xml()
        {
            return SqlType.Type("xml");
        }
    }

    public class SqlTextualDataTypeOptions
    {
        public int CharacterLength { get; }
        
        public SqlTextualDataTypeOptions(int characterLength)
        {
            CharacterLength = characterLength;
        }
    }

    public class SqlTextualDataTypeOptionsFactory
    {
        public static SqlTextualDataTypeOptions FromAmbigiousSqlType(AmbigiousSqlType ambigiousSqlType)
        {
            return new SqlTextualDataTypeOptions(ambigiousSqlType.MaxCharacterText.InsensitiveEquals("max")
                ? int.MaxValue
                : ambigiousSqlType.MaxCharacterLength.Value);
        }
        public static SqlTextualDataTypeOptions FromInformationSchemaColumn(InformationSchemaColumn column)
        {
            return new SqlTextualDataTypeOptions(column.CHARACTER_OCTET_LENGTH.Value);
        }

        public static SqlTextualDataTypeOptions Create(SyntaxParseResult syntaxParseResult)
        {
            var textType = DetermineTextType(syntaxParseResult);
            var textLength = CalculateTextLength(syntaxParseResult, textType);
            return new SqlTextualDataTypeOptions(textLength);
        }

        private static TextType DetermineTextType(SyntaxParseResult syntaxParseResult)
        {
            if (syntaxParseResult.SqlTypeName.Equals("char") || syntaxParseResult.SqlTypeName.Equals("varchar"))
            {
                return TextType.NonUnicode;
            }
            if (syntaxParseResult.SqlTypeName.Equals("nchar") || syntaxParseResult.SqlTypeName.Equals("nvarchar"))
            {
                return TextType.Unicode;
            }
            throw new ArgumentException($"Type, {syntaxParseResult.SqlTypeName}, was non of the expected values " +
                                        $"of char, varchar, nchar, or nvarchar");
        }

        private static int CalculateTextLength(SyntaxParseResult syntaxParseResult, TextType nonUnicode)
        {
            var textLengthSpecification = DetermineTextLengthSpecification(syntaxParseResult);
            switch (textLengthSpecification)
            {
                case SqlTextLengthSpecification.NotSpecified:
                    return 1;

                case SqlTextLengthSpecification.Numeric:
                    ValidateTextLength(nonUnicode, syntaxParseResult.Precision1.Value);
                    return syntaxParseResult.Precision1.Value;

                case SqlTextLengthSpecification.Max:
                    ValidateSqlTypeIsVarcharOrNvarchar(syntaxParseResult);
                    return Int32.MaxValue;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static SqlTextLengthSpecification DetermineTextLengthSpecification(SyntaxParseResult syntaxParseResult)
        {
            if (syntaxParseResult.Precision1.HasValue)
            {
                return SqlTextLengthSpecification.Numeric;
            }
            if ("max".InsensitiveEquals(syntaxParseResult.Text))
            {
                return SqlTextLengthSpecification.Max;
            }
            return SqlTextLengthSpecification.NotSpecified;
        }

        private static void ValidateTextLength(TextType textType, int value)
        {
            switch (textType)
            {
                case TextType.NonUnicode:
                    value.Should().BeInRange(1, 8000);
                    break;
                case TextType.Unicode:
                    value.Should().BeInRange(1, 4000);
                    break;
            }
        }

        private static void ValidateSqlTypeIsVarcharOrNvarchar(SyntaxParseResult syntaxParseResult)
        {
            syntaxParseResult.SqlTypeName.Should().BeOneOf("varchar", "nvarchar");
        }

        private enum SqlTextLengthSpecification
        {
            NotSpecified,
            Numeric,
            Max
        }

        private enum TextType
        {
            NonUnicode,
            Unicode
        }
    }
}