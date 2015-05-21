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
        
        public static SqlType TextType(string typeName, int? precision)
        {
            return new SqlType(typeName) { MaximumCharLength = precision};
        }
    }
}