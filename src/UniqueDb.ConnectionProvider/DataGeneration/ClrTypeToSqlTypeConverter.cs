using System;
using System.Collections.Generic;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class ClrTypeToSqlTypeConverter
    {
        public static IDictionary<string, SqlType> DefaultClrToSqlTypeMap { get; set; } = new Dictionary<string, SqlType>()
        {
            //Text
            { "char", SqlTypeFactory.NVarChar(new SqlTextualDataTypeOptions(1))},
            { "string", SqlTypeFactory.NVarChar(new SqlTextualDataTypeOptions(4000)) },
            { "char[]", SqlTypeFactory.NVarChar(new SqlTextualDataTypeOptions(4000)) },

            //Numeric
            { "boolean", SqlTypeFactory.Bit() },
            { "byte", SqlTypeFactory.TinyInt() },
            { "int16", SqlTypeFactory.SmallInt() },
            { "int32", SqlTypeFactory.Int() },
            { "int64", SqlTypeFactory.BigInt() },
            { "sbyte", SqlTypeFactory.SmallInt() },
            { "uint16", SqlTypeFactory.Int() },
            { "uint32", SqlTypeFactory.BigInt() },
            { "uint64", SqlTypeFactory.Decimal(20,0) },
            { "decimal", SqlTypeFactory.Decimal(29,4) },
            { "single", SqlTypeFactory.Real() },
            { "double", SqlTypeFactory.Float() },

            //DateTime
            { "datetime", SqlTypeFactory.DateTime2(7) },
            { "datetimeoffset", SqlTypeFactory.DateTimeOffset() },
            { "timespan", SqlTypeFactory.Time() },
        };

        public static NullableSqlType Convert(Type clrType)
        {
            ValidateClrType(clrType);
            var isNullable = IsClrTypeNullable(clrType);
            clrType = GetClrType(clrType);
            var sqlType = GetSqlType(clrType);
            var nullableSqlType = new NullableSqlType(sqlType, isNullable);
            return nullableSqlType;
        }

        private static void ValidateClrType(Type clrType)
        {
            if (clrType.IsGenericType && !clrType.Name.Contains("Nullable"))
                throw new ArgumentException(
                    $"Provided type, {clrType.Name}, is generic type that is not a nullable generic type.  This conversion requires non-generic types or nullable types.");
        }

        private static SqlType GetSqlType(Type clrType)
        {
            SqlType sqlType = null;
            var matchFound = DefaultClrToSqlTypeMap.TryGetValue(clrType.Name.ToLower(), out sqlType);

            if(!matchFound && clrType.IsEnum) return SqlTypeFactory.Int();
            if(!matchFound) throw new ArgumentException($"No match found for CLR Type: {clrType.Name}");

            return sqlType;
        }

        private static Type GetClrType(Type clrType)
        {
            var underlyingType = clrType.Name.Contains("Nullable")
                ? Nullable.GetUnderlyingType(clrType)
                : clrType;
            return underlyingType;
        }

        private static bool IsClrTypeNullable(Type clrType)
        {
            return clrType.IsGenericType && clrType.Name.Contains("Nullable");
        }
    }
}