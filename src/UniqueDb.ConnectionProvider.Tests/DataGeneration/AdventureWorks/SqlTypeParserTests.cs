using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration.AdventureWorks
{
    public class SqlTypeParserTests
    {
        [Fact()]
        [Trait("Category", "Instant")]
        public void SimpleName_Test()
        {
            var result = SqlTypeNameParser.Parse("int");
            result.TypeName.Should().Be("int");
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void DoublePrecisionNumber_Test()
        {
            var result = SqlTypeNameParser.Parse("decimal(18,8)");
            result.TypeName.Should().Be("decimal");
            result.NumericPrecision.Should().Be(18);
            result.NumericScale.Should().Be(8);
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void DoublePrecisionNumberWithSinglePrecision_Test()
        {
            var result = SqlTypeNameParser.Parse("float(18)");
            result.TypeName.Should().Be("float");
            result.NumericPrecision.Should().Be(18);
        }

        [Theory()]
        [InlineData("date", "date", null, null, null, null)]
        [InlineData("datetime", "datetime", null, null, null, null)]
        [InlineData("datetime2", "datetime2", null, null, null, null)]
        [InlineData("datetime2(4)", "datetime2", null, null, 4, null)]
        [Trait("Category", "Instant")]
        public void DateTests(string input, string typeName, int? numericPrecision,
            int? numericScale, int? dateTimePrecision, int? charLength)
        {
            var result = SqlTypeNameParser.Parse(input);

            Assert.Equal(result.TypeName, typeName);
            Assert.Equal(result.NumericPrecision, numericPrecision);
            Assert.Equal(result.NumericScale, numericScale);
            Assert.Equal(result.DateTimePrecision, dateTimePrecision);
            Assert.Equal(result.MaximumCharLength, charLength);
        }

        [Theory()]
        [InlineData("char", "char", null, null, null, null)]
        [InlineData("char(15)", "char", null, null, null, 15)]
        [InlineData("char(90000)", "char", null, null, null, 90000)]
        [InlineData("nchar", "nchar", null, null, null, null)]
        [InlineData("nchar(4)", "nchar", null, null, null, 4)]
        [InlineData("varchar", "varchar", null, null, null, null)]
        [InlineData("varchar(4)", "varchar", null, null, null, 4)]
        [InlineData("nvarchar", "nvarchar", null, null, null, null)]
        [InlineData("nvarchar(4)", "nvarchar", null, null, null, 4)]
        [InlineData("text", "text", null, null, null, null)]
        [Trait("Category", "Instant")]
        public void TextTests(string input, string typeName, int? numericPrecision,
            int? numericScale, int? dateTimePrecision, int? charLength)
        {
            var result = SqlTypeNameParser.Parse(input);

            Assert.Equal(result.TypeName, typeName);
            Assert.Equal(result.NumericPrecision, numericPrecision);
            Assert.Equal(result.NumericScale, numericScale);
            Assert.Equal(result.DateTimePrecision, dateTimePrecision);
            Assert.Equal(result.MaximumCharLength, charLength);
        }

        [Fact]
        public void OtherBuiltInTypesTests()
        {
            var otherBuiltInTypes = new List<string>
            {
                "binary", "varbinary", "image",
                "cursor", "hierarchyid", "sql_variant", "table", "timestamp", "uniqueidentifier", "xml",
            };

            foreach (var otherBuiltInType in otherBuiltInTypes)
            {
                var result = SqlTypeNameParser.Parse(otherBuiltInType);

                Assert.Equal(result.TypeName, otherBuiltInType);
                Assert.Equal(result.NumericPrecision, null);
                Assert.Equal(result.NumericScale, null);
                Assert.Equal(result.DateTimePrecision, null);
                Assert.Equal(result.MaximumCharLength, null);
            }
        }
    }
}
