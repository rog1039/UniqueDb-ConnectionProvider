using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit.Extensions;

namespace UniqueDb.ConnectionProvider.Tests
{
    public class DataAnnotationDefinitionNumericRangeTests
    {
        private const decimal _17_3 = 99999999999999.999m;
        [Theory]
        [MemberData("TestCases")]
        [Trait("Category", "Instant")]
        public void TestWithDifferentPrecisionsAndScale(int scale, int precision, decimal lowerBound, decimal upperBound)
        {
            var dataAnnotationDefinitionNumericRange = new DataAnnotationDefinitionNumericRange(scale, precision);
            Console.WriteLine(dataAnnotationDefinitionNumericRange.ToAttributeString());
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
}
