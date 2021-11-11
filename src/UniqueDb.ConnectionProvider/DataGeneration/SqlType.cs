using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public class SqlType
{
    public string TypeName          { get; set; }
    public int?   MaximumCharLength { get; set; }
    public string MaximumCharLengthString => MaximumCharLength == null 
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

public class SqlTypeConverter
{
    public static SqlType FromInformationSchemaColumn(InformationSchemaColumn col)
    {
        if (SqlTypes.IsDateTime(col.DATA_TYPE))
        {
            return SqlType.DateTime(col.DATA_TYPE, col.DATETIME_PRECISION);
        }
        if (SqlTypes.IsApproximateNumeric(col.DATA_TYPE))
        {
            if (col.DATA_TYPE == "float") return SqlTypeFactory.Float(col.NUMERIC_PRECISION.Value);
            if (col.DATA_TYPE == "real") return SqlTypeFactory.Real();
        }
        if (SqlTypes.IsExactNumeric(col.DATA_TYPE))
        {
            return SqlType.ExactNumericType(col.DATA_TYPE, col.NUMERIC_PRECISION, col.NUMERIC_PRECISION_RADIX);
        }
        if (SqlTypes.IsCharType(col.DATA_TYPE))
        {
            return SqlType.TextType(col.DATA_TYPE, col.CHARACTER_MAXIMUM_LENGTH);
        }

        if (col.DATA_TYPE.Equals("uniqueidentifier", StringComparison.InvariantCultureIgnoreCase))
        {
            return SqlType.Type(col.DATA_TYPE);
        }
        if (col.DATA_TYPE.Equals("varbinary", StringComparison.InvariantCultureIgnoreCase))
        {
            return SqlType.Type(col.DATA_TYPE);
        }
        throw new NotImplementedException($"Unknown type {col.DATA_TYPE}");
    }
}