using System.Diagnostics;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider.CSharpGeneration;

public static class DoubleTypeRangeCalculator
{
    public static NumericRange CalculateRange(int numericPrecision, int? numericScale)
    {
        int numericScaleNotNull = numericScale ?? 0;
        try
        {
            var size             = numericPrecision - numericScaleNotNull;
            var upperBoundString = "9".Repeat(size) + "." + "9".Repeat(numericScaleNotNull);
                
            var upperBound = double.Parse(upperBoundString);
            var lowerBound = -upperBound;
            return new NumericRange(lowerBound, upperBound);

        }
        catch (OverflowException e)
        {
            Console.WriteLine(e);
            Debugger.Break();
            return new NumericRange(double.MinValue, double.MaxValue);
        }
    }
}