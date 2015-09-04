using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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

        [Fact()]
        [Trait("Category", "Instant")]
        public void TestAttributes()
        {
            var properties = typeof (SampleClassToCreateTableFor).GetProperties();
            var property = properties.Single(p => p.Name == "SomeString");
            var attributes = property
                .GetCustomAttributes(false)
                .Where(att => att.GetType().Namespace.Contains("System.ComponentModel.DataAnnotations"));

            foreach (var attribute in attributes)
            {
                Console.WriteLine(attribute.GetType().FullName);
            }
        }
    }

    public class SampleClassToCreateTableFor
    {
        public TestEnum TestEnum { get; set; }
        public IList<int> SomeList { get; set; }
        [StringLength(20)]
        [MaxLength(10)]
        public string SomeString { get; set; }
    }

    public enum TestEnum
    {
        Value1,
        Value2,
        Value3
    }
}