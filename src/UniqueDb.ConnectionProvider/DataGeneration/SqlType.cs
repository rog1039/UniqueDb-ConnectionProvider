namespace UniqueDb.ConnectionProvider.DataGeneration;

public class SqlType
{
    public string TypeName          { get; set; }
    
    public int?   MaximumCharLength { get; set; }
    public string? MaximumCharLengthString => MaximumCharLength == null 
        ? null 
        : ((MaximumCharLength.Value == -1) ? "Max" : MaximumCharLength.ToString());
    
    public int? NumericPrecision           { get; set; }
    public int? NumericScale               { get; set; }
    public int? FractionalSecondsPrecision { get; set; }
    public int? Mantissa                   { get; set; }


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
        return new SqlType(typeName) { MaximumCharLength = precision };
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
            /*
             * Exact numerics
             */
            case "numeric":
            case "decimal": return $"{TypeName}({NumericPrecision},{NumericScale})";

            
            /*
             * Approximate Numerics
             */
            case "real":
            case "float": return $"{TypeName}({Mantissa})";

            
            /*
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

            
            /*
             * Date/Time...
             */
            case "datetime2":
            case "datetimeoffset":
            case "time":
                return $"{TypeName}({FractionalSecondsPrecision})";

            
            /*
             * With char lengths...
             */
            case "binary":
            case "varbinary":
                
            case "char":
            case "varchar":
            case "nchar":
            case "nvarchar":
            {
                var length = MaximumCharLength == -1 ? "MAX" : MaximumCharLength.ToString();
                return $"{TypeName}({length})";
            }
            

            default:
                throw new InvalidDataException($"No cases defined to translate data type: {TypeName}.");
        }
    }

    [Obsolete]
    public string ToStringObsolete()
    {
        if (SqlTypes.IsCharType(TypeName))
        {
            var charLength = MaximumCharLength.HasValue && MaximumCharLength.Value > 0
                ? MaximumCharLength.Value.ToString()
                : "MAX";

            return $"{TypeName}({charLength})";
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