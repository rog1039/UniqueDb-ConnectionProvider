using UniqueDb.ConnectionProvider.Converters;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public static class ClrTypeToSqlTypeConverter
{
    public static IDictionary<string, SqlType> DefaultClrToSqlTypeMap { get; set; } = new Dictionary<string, SqlType>()
    {
        //Text
        { "char", SqlTypeFactory.NVarChar(new SqlTextLength(1))},
        { "string", SqlTypeFactory.NVarChar(new SqlTextLength(4000)) },
        { "char[]", SqlTypeFactory.NVarChar(new SqlTextLength(4000)) },

        //Numeric
        { "boolean", SqlTypeFactory.Bit() },
        { "byte", SqlTypeFactory.TinyInt() },
        { "int16", SqlTypeFactory.SmallInt() },
        { "int32", SqlTypeFactory.Int() },
        { "int64", SqlTypeFactory.BigInt() },
        { "sbyte", SqlTypeFactory.SmallInt() },
        { "uint16", SqlTypeFactory.Int() },
        { "uint32", SqlTypeFactory.BigInt() },
        { "uint64", SqlTypeFactory.Decimal(20,  0) },
        { "decimal", SqlTypeFactory.Decimal(29, 4) },
        { "single", SqlTypeFactory.Real() },
        { "double", SqlTypeFactory.Float() },

        //DateTime
        { "datetime", SqlTypeFactory.DateTime2(7) },
        { "datetimeoffset", SqlTypeFactory.DateTimeOffset() },
        { "timespan", SqlTypeFactory.Time() },
    };

    public static bool CanTranslateToSqlType(Type clrType)
    {
        // var isNonNullableGeneric = IsNonNullableGeneric(clrType);
        // if (isNonNullableGeneric) return false;
            
        clrType = StripNullableOuterGeneric(clrType);
        if (clrType.IsEnum) return true;
        var matchingSqlTypeExists = DefaultClrToSqlTypeMap.ContainsKey(clrType.Name.ToLower());
        return matchingSqlTypeExists;
    }

    public static bool IsNonNullableGeneric(Type clrType)
    {
        var isGeneric      = clrType.IsGenericType;
        var isNotANullable = !clrType.Name.Contains("Nullable");

        return isGeneric && isNotANullable;
    }
        
    public static NullableSqlType Convert(Type clrType)
    {
        ValidateClrTypeIsTranslatable(clrType);
        var underlyingType = StripNullableOuterGeneric(clrType);
        var sqlType        = GetSqlType(underlyingType);
            
        var isNullable      = IsClrTypeNullable(clrType);
        var nullableSqlType = new NullableSqlType(sqlType, isNullable);
        return nullableSqlType;
    }

    private static void ValidateClrTypeIsTranslatable(Type clrType)
    {
        // if (IsNonNullableGeneric(clrType))
        if (!CanTranslateToSqlType(clrType))
            throw new ArgumentException($"Can't translate ClrType {clrType.FullName}");
        // throw new ArgumentException(
        //     $"Provided type, {clrType.Name}, is generic type that is not a nullable generic type.  This conversion requires non-generic types or nullable types.");
    }

    private static SqlType GetSqlType(Type clrType)
    {
        SqlType sqlType    = null;
        var     matchFound = DefaultClrToSqlTypeMap.TryGetValue(clrType.Name.ToLower(), out sqlType);

        if(!matchFound && clrType.IsEnum) return SqlTypeFactory.Int();
        if(!matchFound) throw new ArgumentException($"No match found for CLR Type: {clrType.Name}");

        return sqlType;
    }

    private static Type StripNullableOuterGeneric(Type clrType)
    {
        var underlyingType = Nullable.GetUnderlyingType(clrType);
        return underlyingType ?? clrType;
    }

    private static bool IsClrTypeNullable(Type clrType)
    {
        var underlyingType = Nullable.GetUnderlyingType(clrType);
        return underlyingType is null ? false : true;
    }
}