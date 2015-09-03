using System;
using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class SqlType
    {
        public string TypeName { get; set; }
        public int? MaximumCharLength { get; set; }
        public int? NumericPrecision { get; set; }
        public int? NumericScale { get; set; }
        public int? FractionalSecondsPrecision { get; set; }
        public int? Mantissa { get; set; }
        

        protected SqlType(string typeName)
        {
            TypeName = typeName;
        }

        public static SqlType Type(string typeName)
        {
            return new SqlType(typeName);
        }

        public static SqlType TextType(string typeName, int? precision)
        {
            return new SqlType(typeName) { MaximumCharLength = precision};
        }
        
        public static SqlType ApproximateNumeric(string typeName, int? mantissa = 53)
        {
            return new SqlType(typeName) { Mantissa = mantissa };
        }

        public static SqlType ExactNumericType(string typeName, int? numericPrecision, int? numericScale)
        {
            return new SqlType(typeName) { NumericPrecision = numericPrecision, NumericScale = numericScale};
        }

        public static SqlType DateTime(string typeName, int? fractionalSecondsPrecision)
        {
            return new SqlType(typeName) { FractionalSecondsPrecision = fractionalSecondsPrecision};
        }
        public override string ToString()
        {
            if (SqlTypes.IsCharType(TypeName))
            {
                return TypeName + (MaximumCharLength.HasValue ? $"({MaximumCharLength.Value})" : string.Empty);
            }
            if (SqlTypes.IsApproximateNumeric(TypeName))
            {
                return TypeName + (Mantissa.HasValue ? $"({Mantissa.Value})" : string.Empty);
            }
            if (SqlTypes.IsExactNumeric(TypeName))
            {
                if (NumericPrecision.HasValue && NumericScale.HasValue)
                {
                    return $"{TypeName}({NumericPrecision},{NumericScale})";
                }
                if (NumericPrecision.HasValue)
                {
                    return $"{TypeName}({NumericPrecision})";
                }
                return TypeName;
            }
            if (SqlTypes.IsDateTime(TypeName))
            {
                return FractionalSecondsPrecision.HasValue && SqlTypes.IsDateTimeWithPrecision(TypeName)
                    ? $"{TypeName}({FractionalSecondsPrecision})"
                    : TypeName;
            }
            return TypeName;
            throw new ArgumentException($"Conversion for the type {TypeName} is not defined.");
        }
    }
}