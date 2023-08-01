using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata.InformationSchema;

namespace UniqueDb.ConnectionProvider.CoreTypes;

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
      if (numberOfBits is < 1 or > 53)
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

   public static SqlType Create(string typeName, 
                                int precision, 
                                int scale, 
                                int maxCharLength)
   {
      switch (typeName)
      {
         /* *************************************************************************
          * Exact numerics
          */
         case "numeric":
         case "decimal":
            return SqlTypeFactory.Numeric(precision, scale);


         /* *************************************************************************
          * Approximate Numerics
          */
         case "real":
         case "float":
            throw new NotImplementedException();

         /* *************************************************************************
          * Just the type names
          */
         case "bit":
         case "tinyint":
         case "smallint":
         case "int":
         case "bigint":
            return new SqlType(typeName);

         case "date":
         case "datetime":
         case "smalldatetime":

         case "xml":
         case "uniqueidentifier":
         case "timestamp":
         case "rowversion":

            return new SqlType(typeName);


         /* *************************************************************************
          * Date/Time...
          */
         case "datetime2":
         case "datetimeoffset":
         case "time":

            return SqlType.DateTime(typeName, scale);


         /* *************************************************************************
          * With char lengths...
          */
         case "binary":
         case "varbinary":

         case "char":
         case "varchar":
         case "nchar":
         case "nvarchar":

            return SqlType.TextType(typeName, maxCharLength);


         default:
            throw new InvalidDataException($"No cases defined to translate data type: {typeName}."); 
      }
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

   public static implicit operator SqlTextLength(int maxLength)
   {
      return new SqlTextLength(maxLength);
   }
}

public static class SqlTextLengthFactory
{
   public static SqlTextLength FromSqlTypeSpecification(SqlTypeSpecification sqlTypeSpecification)
   {
      var maxCharacterLength = sqlTypeSpecification.MaxCharacterText.InsensitiveEquals("max")
         ? int.MaxValue
         : sqlTypeSpecification.MaxCharacterLength.Value;
      
      return new SqlTextLength(maxCharacterLength);
   }

   public static SqlTextLength FromInformationSchemaColumn(SISColumn column)
   {
      return new SqlTextLength(column.CHARACTER_OCTET_LENGTH.Value);
   }
}