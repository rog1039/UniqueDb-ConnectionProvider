using System;
using System.Reflection;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud
{
    public static class SqlValueEncoder
    {
        public static string ConvertPropertyToSqlString(object o, PropertyInfo x)
        {
            object value = x.GetValue(o, null);
            if (x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return value == null
                    ? "NULL"
                    : EncodeValueAsTextForSqlInsert(value);
            }
            return EncodeValueAsTextForSqlInsert(value);
        }

        public static string EncodeValueAsTextForSqlInsert(object value)
        {
            if (value is string)
            {
                return $"'{value}'";
            }
            if (IsNumericType(value.GetType()))
            {
                return $"{value}";
            }
            if (value is DateTime)
            {
                var dateTimeString = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss");
                return $"'{dateTimeString}'";
            }
            if (value is Guid)
            {
                return $"'{value}'";
            }
            throw new NotImplementedException(
                $"Don't know how to convert {value.GetType()} to a string for the SQL statement.");
        }

        private static bool IsNumericType(Type type)
        {
            if (type == null)
            {
                return false;
            }

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return true;
                case TypeCode.Object:
                    if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        return IsNumericType(Nullable.GetUnderlyingType(type));
                    }
                    return false;
            }
            return false;
        }
    }
}