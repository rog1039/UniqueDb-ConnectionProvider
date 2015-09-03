using System;
using System.Collections.Generic;
using UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class GetSqlCreateTableScriptsTests
    {
        [Fact()]
        [Trait("Category", "Instant")]
        public void CreateScriptFromCSharpClass()
        {
            var response = CreateTableScriptProvider.GetCreateTableScript<DescribeResultSetRow>();
            Console.WriteLine(response);
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void TestWithSimpleClass()
        {
            var response = CreateTableScriptProvider.GetCreateTableScript<SampleClassToCreateTableFor>();
            Console.WriteLine(response);
        }
    }

    public class SampleClassToCreateTableFor
    {
        public TestEnum TestEnum { get; set; }
        public IList<int> SomeList { get; set; } 
    }

    public enum TestEnum
    {
        Value1,
        Value2,
        Value3
    }
}