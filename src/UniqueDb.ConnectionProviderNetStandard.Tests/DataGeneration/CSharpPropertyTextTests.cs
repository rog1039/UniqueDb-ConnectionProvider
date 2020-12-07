using System;
using System.Collections.Generic;
using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class CSharpPropertyTextGeneratorTests
    {

        [Theory]
        [MemberData(nameof(TestCases))]
        [Trait("Category", "Instant")]
        public void RunTestCases(TestCase testCase)
        {
            var property = new CSharpProperty()
            {
                DataType = testCase.DataType,
                IsNullable = testCase.IsNullable,
                Name = testCase.PropertyName
            };
            
            var text = new CSharpPropertyTextGenerator(property).Generate();
            Console.WriteLine(text);
            text.Trim().Should().Be(testCase.ExpectedOutput);
        }

        public static IEnumerable<object[]> TestCases()
        {
            yield return new[] {new TestCase("int", false, "Name", "public int Name { get; set; }")};
            yield return new[] {new TestCase("int", true, "Name", "public int? Name { get; set; }")};
            yield return new[] {new TestCase("Type", false, "Name", "public Type Name { get; set; }")};
            yield return new[] {new TestCase("Type", true, "Name", "public Type Name { get; set; }")};
        }

        public class TestCase
        {
            public TestCase(string dataType, bool isNullable, string propertyName, string expectedOutput)
            {
                DataType = dataType;
                IsNullable = isNullable;
                PropertyName = propertyName;
                ExpectedOutput = expectedOutput;
            }

            public string DataType { get; set; }
            public bool IsNullable { get; set; }
            public string PropertyName { get; set; }
            public string ExpectedOutput { get; set; }
        }
    }

    public class CSharpPropertyFactoryFromSqlColumnTests
    {
        [Fact()]
        [Trait("Category", "Instant")]
        public void TestDataAnnotationDefinitionNumericRange()
        {
            var sqlColumn = new SqlColumn()
            {
                SqlDataType = SqlTypeNumberBase.FromBounds("int", 100, 200),
                IsNullable = true,
                Name = "PropertyName"
            };

            var cSharpProperty = CSharpPropertyFactoryFromSqlColumn.ToCSharpProperty(sqlColumn);
            Console.WriteLine(cSharpProperty);
        }
    }
}
