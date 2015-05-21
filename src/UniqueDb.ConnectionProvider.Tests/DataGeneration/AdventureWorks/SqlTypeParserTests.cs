using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
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
            var result = SyntaxParseResultToSqlTypeConverter.Parse("int");
            result.TypeName.Should().Be("int");
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void DoublePrecisionNumber_Test()
        {
            var result = SyntaxParseResultToSqlTypeConverter.Parse("decimal(18,8)");
            result.TypeName.Should().Be("decimal");
            result.NumericPrecision.Should().Be(18);
            result.NumericScale.Should().Be(8);
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void DoublePrecisionNumberWithSinglePrecision_Test()
        {
            var result = SyntaxParseResultToSqlTypeConverter.Parse("decimal(18)");
            result.TypeName.Should().Be("decimal");
            result.NumericPrecision.Should().Be(18);
            result.NumericScale.Should().Be(0);
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void Float_Test()
        {
            var result = SyntaxParseResultToSqlTypeConverter.Parse("float(18)");
            result.TypeName.Should().Be("float");
            result.Mantissa.Should().Be(18);
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void Real_Test()
        {
            var result = SyntaxParseResultToSqlTypeConverter.Parse("real");
            result.TypeName.Should().Be("real");
            result.Mantissa.Should().Be(24);
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
            var result = SyntaxParseResultToSqlTypeConverter.Parse(input);

            Assert.Equal(result.TypeName, typeName);
            Assert.Equal(result.NumericPrecision, numericPrecision);
            Assert.Equal(result.NumericScale, numericScale);
            Assert.Equal(result.FractionalSecondsPrecision, dateTimePrecision);
            Assert.Equal(result.MaximumCharLength, charLength);
        }

        [Theory()]
        [InlineData("char", "char", null, null, null, 1)]
        [InlineData("char(15)", "char", null, null, null, 15)]
        [InlineData("nchar", "nchar", null, null, null, 1)]
        [InlineData("nchar(4)", "nchar", null, null, null, 4)]
        [InlineData("varchar", "varchar", null, null, null, 1)]
        [InlineData("varchar(4)", "varchar", null, null, null, 4)]
        [InlineData("nvarchar", "nvarchar", null, null, null, 1)]
        [InlineData("nvarchar(4)", "nvarchar", null, null, null, 4)]
        [InlineData("nvarchar(max)", "nvarchar", null, null, null, int.MaxValue)]
        [InlineData("text", "text", null, null, null, null)]
        [Trait("Category", "Instant")]
        public void TextTests(string input, string typeName, int? numericPrecision,
            int? numericScale, int? dateTimePrecision, int? charLength)
        {
            var result = SyntaxParseResultToSqlTypeConverter.Parse(input);

            Assert.Equal(typeName, result.TypeName);
            Assert.Equal(numericPrecision, result.NumericPrecision);
            Assert.Equal(numericScale, result.NumericScale);
            Assert.Equal(dateTimePrecision, result.FractionalSecondsPrecision);
            Assert.Equal(charLength, result.MaximumCharLength);
        }

        [Fact]
        [Trait("Category", "Instant")]
        public void InvalidTextLengthShouldThrowExceptionTest()
        {
            var throwWhenCharLengthIsInvalid = new Action(() =>
            {
                var result = SyntaxParseResultToSqlTypeConverter.Parse("char(90000)");
            });
            throwWhenCharLengthIsInvalid.ShouldThrow<Exception>();


            var throwWhenCharLengthIsMax = new Action(() =>
            {
                var result = SyntaxParseResultToSqlTypeConverter.Parse("nchar(max)");
            });
            throwWhenCharLengthIsMax.ShouldThrow<Exception>();
        }

        [Fact]
        [Trait("Category", "Instant")]
        public void OtherBuiltInTypesTests()
        {
            var otherBuiltInTypes = new List<string>
            {
                "binary", "varbinary", "image",
                "cursor", "hierarchyid", "sql_variant", "table", "timestamp", "uniqueidentifier", "xml",
            };

            foreach (var otherBuiltInType in otherBuiltInTypes)
            {
                var result = SyntaxParseResultToSqlTypeConverter.Parse(otherBuiltInType);

                Assert.Equal(result.TypeName, otherBuiltInType);
                Assert.Equal(result.NumericPrecision, null);
                Assert.Equal(result.NumericScale, null);
                Assert.Equal(result.FractionalSecondsPrecision, null);
                Assert.Equal(result.MaximumCharLength, null);
            }
        }
    }
}
