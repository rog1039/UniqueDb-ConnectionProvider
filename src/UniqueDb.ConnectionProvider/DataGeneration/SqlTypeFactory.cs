using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration;

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
      if (precision is < 1 or > 38)
         throw new ArgumentException("Precision must be between 1 and 38 inclusive", nameof(precision));
      
      if (scale.HasValue && (scale < 0 || scale > precision))
         throw new ArgumentException("Scale, if provided, must be between 0 and 38 inclusive", nameof(scale));

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
      return SqlTypeNumberBase.FromBounds("smallmoney", -214748.3648, 214748.3647);
   }


   /****   Approximate Numerics   ****/
   public static SqlType Float(int numberOfBits = 53)
   {
      if (numberOfBits is < 1 or > 53 )
         throw new ArgumentException("Mantissa must be between 1 and 53 inclusive", nameof(numberOfBits));
      
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

   public static SqlType DateTime2(int? fractionalSecondsPrecision = 7)
   {
      if (fractionalSecondsPrecision is < 0 or > 7)
         throw new ArgumentException("FractionalSecondsPrecision must be between 0 and 7 inclusive.",
            nameof(fractionalSecondsPrecision));
      
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
   public static SqlType Char(SqlTextLength options)
   {
      return SqlType.TextType("char", options.CharacterLength);
   }

   public static SqlType VarChar(SqlTextLength options)
   {
      return SqlType.TextType("varchar", options.CharacterLength);
   }

   public static SqlType NChar(SqlTextLength options)
   {
      return SqlType.TextType("nchar", options.CharacterLength);
   }

   public static SqlType NVarChar(SqlTextLength options)
   {
      return SqlType.TextType("nvarchar", options.CharacterLength);
   }

   public static SqlType Xml()
   {
      return SqlType.Type("xml");
   }
}

public class SqlTextLength
{
   public int CharacterLength { get; }

   public SqlTextLength(int characterLength)
   {
      CharacterLength = characterLength;
   }

   public static SqlTextLength Max => new SqlTextLength(int.MaxValue);
}

public static class SqlTextLengthFactory
{
   public static SqlTextLength FromAmbiguousSqlType(AmbiguousSqlType ambiguousSqlType)
   {
      return new SqlTextLength(ambiguousSqlType.MaxCharacterText.InsensitiveEquals("max")
         ? int.MaxValue
         : ambiguousSqlType.MaxCharacterLength.Value);
   }

   public static SqlTextLength FromInformationSchemaColumn(SISColumn column)
   {
      return new SqlTextLength(column.CHARACTER_OCTET_LENGTH.Value);
   }

   public static SqlTextLength Create(SyntaxParseResult syntaxParseResult)
   {
      var textType   = DetermineTextType(syntaxParseResult);
      var textLength = CalculateTextLength(syntaxParseResult, textType);
      return new SqlTextLength(textLength);
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
            if (value < 1 || value > 8000)
               throw new ArgumentException("Value must be between 1 and 8000.", nameof(value));
            break;
         case TextType.Unicode:
            if (value < 1 || value > 4000)
               throw new ArgumentException("Value must be between 1 and 4000.", nameof(value));
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