namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public class SqlTypeNumberBase : SqlType
    {
        private SqlTypeNumberBase(string typeName) : base(typeName)
        {
        }

        public static SqlTypeNumberBase FromPrecisionAndScale(string typeName, int precision, int? scale)
        {
            var sqlTypeNumberBase = new SqlTypeNumberBase(typeName);
            var scale2 = scale ?? 0;
            var range = DecimalTypeRangeCalculator.CalculateRange(precision, scale2);
            sqlTypeNumberBase.UpperBound = range.UpperBound;
            sqlTypeNumberBase.LowerBound = range.LowerBound;
            sqlTypeNumberBase.NumericPrecision = precision;
            sqlTypeNumberBase.NumericScale = scale2;
            return sqlTypeNumberBase;
        }

        public static SqlTypeNumberBase FromBounds(string typeName, decimal lowerBound, decimal upperBound)
        {
            var sqlTypeNumberBase = new SqlTypeNumberBase(typeName);
            sqlTypeNumberBase.LowerBound = lowerBound;
            sqlTypeNumberBase.UpperBound = upperBound;
            return sqlTypeNumberBase;
        }

        public decimal UpperBound { get; set; }
        public decimal LowerBound { get; set; }
    }
}