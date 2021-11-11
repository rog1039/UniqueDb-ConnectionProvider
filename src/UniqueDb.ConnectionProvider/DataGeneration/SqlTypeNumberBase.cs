using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public class SqlTypeNumberBase : SqlType
{
    private SqlTypeNumberBase(string typeName) : base(typeName)
    {
    }

    public static SqlTypeNumberBase FromPrecisionAndScale(string typeName, int precision, int? scale)
    {
        var sqlTypeNumberBase = new SqlTypeNumberBase(typeName);
        var scale2            = scale ?? 0;
        var range             = DoubleTypeRangeCalculator.CalculateRange(precision, scale2);

        if (typeName.InsensitiveEquals("decimal"))
        {
            try
            {
                range.UpperBound = (double)(decimal)range.UpperBound;
            }
            catch (Exception)
            {

                range.UpperBound = (double)decimal.MaxValue;
            }
            try
            {
                range.LowerBound = (double)(decimal)range.LowerBound;
            }
            catch (Exception)
            {

                range.LowerBound = (double)decimal.MinValue;
            }
        }

        sqlTypeNumberBase.UpperBound       = range.UpperBound;
        sqlTypeNumberBase.LowerBound       = range.LowerBound;
        sqlTypeNumberBase.NumericPrecision = precision;
        sqlTypeNumberBase.NumericScale     = scale2;
        return sqlTypeNumberBase;
    }

    public static SqlTypeNumberBase FromBounds(string typeName, double lowerBound, double upperBound)
    {
        var sqlTypeNumberBase = new SqlTypeNumberBase(typeName);
        sqlTypeNumberBase.LowerBound = lowerBound;
        sqlTypeNumberBase.UpperBound = upperBound;
        return sqlTypeNumberBase;
    }

    public double UpperBound { get; set; }
    public double LowerBound { get; set; }
}