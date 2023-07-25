using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using UniqueDb.ConnectionProvider.SqlMetadata.DescribeResultSet;
using UniqueDb.ConnectionProvider.SqlScripting;
using Xunit;
using Xunit.Abstractions;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class GetSqlCreateTableScriptsTests : UnitTestBaseWithConsoleRedirection
{
    [Fact()]
    [Trait("Category", "Instant")]
    public void CreateScriptFromCSharpClass()
    {
        var response = ClrTypeToSqlDmlCreateStatementGenerator.GetCreateTableScript<DescribeResultSetRow>();
        Console.WriteLine(response);
    }

    [Fact()]
    [Trait("Category", "Instant")]
    public void TestWithSimpleClass()
    {
        var response = ClrTypeToSqlDmlCreateStatementGenerator.GetCreateTableScript<SampleClassToCreateTableFor>();
        Console.WriteLine(response);
    }

    [Fact()]
    [Trait("Category", "Instant")]
    public void TestAttributes()
    {
        var properties = typeof (SampleClassToCreateTableFor).GetProperties();
        var property   = properties.Single(p => p.Name == "SomeString");
        var attributes = property
            .GetCustomAttributes(false)
            .Where(att => att.GetType().Namespace.Contains("System.ComponentModel.DataAnnotations"));

        foreach (var attribute in attributes)
        {
            Console.WriteLine(attribute.GetType().FullName);
        }
    }

    public GetSqlCreateTableScriptsTests(ITestOutputHelper outputHelperHelper) : base(outputHelperHelper) { }
}

public class SampleClassToCreateTableFor
{
    [Key]
    [Column(Order = 100)]
    public int OrderLineNumber { get; set; }
    public TestEnum   TestEnum { get; set; }
    public IList<int> SomeList { get; set; }
    [StringLength(20)]
    [MaxLength(10)]
    public string SomeString { get; set; }
    [Key]
    [Column(Order=10)]
    public int OrderNumber { get; set; }
}

public enum TestEnum
{
    Value1,
    Value2,
    Value3
}