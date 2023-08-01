using UniqueDb.ConnectionProvider.SqlMetadata.SysTables.VeryNiceCopies;

namespace UniqueDb.ConnectionProvider.CoreTypes;

public class SqlType
{
   public string TypeName                   { get; set; }
   public int?   MaximumCharLength          { get; set; }
   public int?   NumericPrecision           { get; set; }
   public int?   NumericScale               { get; set; }
   public int?   Mantissa                   { get; set; }
   public int?   FractionalSecondsPrecision { get; set; }

   public string? MaximumCharLengthString => MaximumCharLength is null
      ? null
      : MaximumCharLength.Value is -1 or int.MaxValue
         ? "Max"
         : MaximumCharLength.ToString();


   protected internal SqlType(string typeName)
   {
      TypeName = typeName;
   }
   
   public static SqlType Type(string typeName)
   {
      return new SqlType(typeName);
   }

   public static SqlType TextType(string typeName, int? maxCharLength)
   {
      return new SqlType(typeName) { MaximumCharLength = maxCharLength };
   }

   public static SqlType ApproximateNumeric(string typeName, int? mantissa = 53)
   {
      return new SqlType(typeName) { Mantissa = mantissa };
   }

   public static SqlType ExactNumericType(string typeName, int? numericPrecision, int? numericScale)
   {
      return new SqlType(typeName) { NumericPrecision = numericPrecision, NumericScale = numericScale };
   }

   public static SqlType DateTime(string typeName, int? fractionalSecondsPrecision)
   {
      return new SqlType(typeName) { FractionalSecondsPrecision = fractionalSecondsPrecision };
   }

   public override string ToString()
   {
      switch (TypeName.ToLower())
      {
         /* *************************************************************************
          * Exact numerics
          */
         case "numeric":
         case "decimal": 
            
            return $"{TypeName}({NumericPrecision},{NumericScale})";


         /* *************************************************************************
          * Approximate Numerics
          */
         case "real":
         case "float": 
            
            return $"{TypeName}({Mantissa})";


         /* *************************************************************************
          * Just the type names
          */
         case "bit":
         case "tinyint":
         case "smallint":
         case "int":
         case "bigint":

         case "date":
         case "datetime":
         case "smalldatetime":

         case "xml":
         case "uniqueidentifier":
         case "timestamp":
         case "rowversion":
            
            return TypeName;


         /* *************************************************************************
          * Date/Time...
          */
         case "datetime2":
         case "datetimeoffset":
         case "time":
            
            return $"{TypeName}({FractionalSecondsPrecision})";


         /* *************************************************************************
          * With char lengths...
          */
         case "binary":
         case "varbinary":

         case "char":
         case "varchar":
         case "nchar":
         case "nvarchar":
            
            return $"{TypeName}({MaximumCharLengthString})";


         default:
            throw new InvalidDataException($"No cases defined to translate data type: {TypeName}.");
      }
   }
}