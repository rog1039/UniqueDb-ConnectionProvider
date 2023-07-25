using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests;

public class RegexExtensionTests
{
    [Fact()]
    [Trait("Category", "Instant")]
    public void MatchRegexWithAnonymousTypeTest()
    {
        var regex = @"(?<WeightLb>\d+)\s{0,4}(?<UoM>(LB|OZ|kg))";
        var input = @"18LB";

        var result = ListExtensionMethods.MakeList(input)
            .MatchRegex(regex, new {WeightLb = "", UoM = ""})
            .Single();
            
        result.WeightLb.Should().Be("18");
        result.UoM.Should().Be("LB");
    }
}