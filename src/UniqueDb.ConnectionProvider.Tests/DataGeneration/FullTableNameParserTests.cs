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
        var parse1  = FullTableNameParser.ParseFullTableName("dbo.Table");
        parse1.Should().Be(new FullTableName("dbo", "Table"));
        
        var parse2  = FullTableNameParser.ParseFullTableName("Table");
        parse2.Should().Be(new FullTableName(null, "Table"));
        
        var parse3  = FullTableNameParser.ParseFullTableName("[A.B]");
        parse3.Should().Be(new FullTableName(null, "A.B"));
        
        var parse4  = FullTableNameParser.ParseFullTableName("dbo.[A.B]");
        parse4.Should().Be(new FullTableName("dbo", "A.B"));
    } 

    public FullTableNameParserTests(ITestOutputHelper outputHelperHelper) : base(outputHelperHelper) { }
}