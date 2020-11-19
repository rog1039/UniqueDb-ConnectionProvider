using System;
using UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;
using Xunit;
using Xunit.Abstractions;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class ClrSqlCreateScriptTests : UnitTestBaseWithConsoleRedirection
    {
        [Fact()]
        [Trait("Category", "Instant")]
        public void TestClassATest()
        {
            var script = ClrTypeToSqlDmlCreateStatementGenerator.GetCreateTableScript<TestClassA>();
            Console.WriteLine(script);
        }
        
        [Fact()]
        [Trait("Category", "Instant")]
        public void TestClassATestUsingRuntimeType()
        {
            var type = new TestClassA().GetType();
            var script = ClrTypeToSqlDmlCreateStatementGenerator.GetCreateTableScript(type);
            Console.WriteLine(script);
        }
        
        public ClrSqlCreateScriptTests(ITestOutputHelper outputHelperHelper) : base(outputHelperHelper)
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