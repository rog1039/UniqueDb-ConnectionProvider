using System;
using UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;
using Xunit;
using Xunit.Abstractions;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class SqlCreateScriptTests : UnitTestBaseWithConsoleRedirection
    {
        [Fact()]
        [Trait("Category", "Instant")]
        public void TestClassATest()
        {
            var script = CreateTableScriptProvider.GetCreateTableScript<TestClassA>();
            Console.WriteLine(script);
        }
        
        public SqlCreateScriptTests(ITestOutputHelper outputHelperHelper) : base(outputHelperHelper)
        {
        }
    }

    public class TestClassA
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TestClassB TestClassB { get; set; }
    }

    public class TestClassB
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}