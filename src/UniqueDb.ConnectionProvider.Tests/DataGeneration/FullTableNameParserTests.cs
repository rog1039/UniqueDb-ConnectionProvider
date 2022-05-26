using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;
using Xunit.Abstractions;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class FullTableNameParserTests : UnitTestBaseWithConsoleRedirection
{
    [Fact()]
    [Trait("Category", "Instant")]
    public void Table()
    {
        var parse1  = DbTableNameWithNullableSchemaParser.ParseFullTableName("dbo.Table");
        parse1.Should().Be(new DbTableNameWithNullableSchema("dbo", "Table"));
        
        var parse2  = DbTableNameWithNullableSchemaParser.ParseFullTableName("Table");
        parse2.Should().Be(new DbTableNameWithNullableSchema(null, "Table"));
        
        var parse3  = DbTableNameWithNullableSchemaParser.ParseFullTableName("[A.B]");
        parse3.Should().Be(new DbTableNameWithNullableSchema(null, "A.B"));
        
        var parse4  = DbTableNameWithNullableSchemaParser.ParseFullTableName("dbo.[A.B]");
        parse4.Should().Be(new DbTableNameWithNullableSchema("dbo", "A.B"));
        
        var parseFail  = () => DbTableNameWithNullableSchemaParser.ParseFullTableName("d.bo.[A.B]");
        parseFail.Should().Throw<Exception>();
    } 

    public FullTableNameParserTests(ITestOutputHelper outputHelperHelper) : base(outputHelperHelper) { }
}