using System;
using System.Data;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class SqlColumn
    {
        public string Name { get; set; }
        public int OrdinalPosition { get; set; }
        public SqlType SqlDataType { get; set; }
        public bool IsNullable { get; set; }
        public string Default { get; set; }
    }

    public class SqlType
    {
        public SqlType() { }

        public SqlType(string typeName)
        {
            TypeName = typeName;
        }

        public SqlType(string typeName, int? numericPrecision)
        {
            TypeName = typeName;
            NumericPrecision = numericPrecision;
        }

        public SqlType(string typeName, int? numericPrecision, int? numericScale)
        {
            TypeName = typeName;
            NumericPrecision = numericPrecision;
            NumericScale = numericScale;
        }

        public static SqlType DateTime2(string typeName, int? precision)
        {
            return new SqlType(typeName) { DateTimePrecision = precision};
        }

        public static SqlType TextType(string typeName, int? precision)
        {
            return new SqlType(typeName) { MaximumCharLength = precision};
        }

        public bool IsSystemDefined { get; set; }
        public string TypeName { get; set; }
        public int? MaximumCharLength { get; set; }
        public int? NumericPrecision { get; set; }
        public int? NumericScale { get; set; }
        public int? DateTimePrecision { get; set; }
    }
}