using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests
{
    public class RegexExtensionTests
    {
        [Fact()]
        [Trait("Category", "Instant")]
        public void MatchRegexWithAnonymousTypeTest()
        {
            var regex = @"(?<WeightLb>\d+)\s{0,4}(?<UoM>(LB|OZ|kg))";
            var input = @"18LB";

            var result = input
                .MakeList()
                .MatchRegex(regex, new {WeightLb = "", UoM = ""})
                .Single();
            
            result.WeightLb.Should().Be("18");
            result.UoM.Should().Be("LB");
        }
    }
}
