using FluentAssertions;
using UniqueDb.ConnectionProvider.CSharpGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests;

public class DataAnnotationDefinitionNumericRangeTests
{
    private const double _17_3 = 99999999999999.999;
    [Theory]
    [MemberData(nameof(TestCases))]
    [Trait("Category", "Instant")]
    public void TestWithDifferentPrecisionsAndScale(int scale, int precision, double lowerBound, double upperBound)
    {
        var dataAnnotationDefinitionNumericRange = DoubleTypeRangeCalculator.CalculateRange(scale, precision);
        dataAnnotationDefinitionNumericRange.LowerBound.Should().Be(lowerBound);
        dataAnnotationDefinitionNumericRange.UpperBound.Should().Be(upperBound);
    }

    public static IEnumerable<object[]> TestCases()
    {
        yield return new object[] {4, null, -9999m, 9999m};
        yield return new object[] {4, 2, -99.99m, 99.99m};
        yield return new object[] {17, 3, -_17_3, _17_3};
    }
}