using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;
using Xunit.Abstractions;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class ClrTypeToSqlTypeConverterTests : UnitTestBaseWithConsoleRedirection
{
    [Fact()]
    [Trait("Category", "Instant")]
    public void NullableInt()
    {
        TestTranslate(typeof(int?), true);
    }

    [Fact()]
    [Trait("Category", "Instant")]
    public void PlainInt()
    {
        TestTranslate(typeof(int), true);
    }
        
    [Fact()]
    [Trait("Category", "Instant")]
    public void String()
    {
        TestTranslate(typeof(string), true);
    }
        
    [Fact()]
    [Trait("Category", "Instant")]
    public void Enum()
    {
        TestTranslate(typeof(TestEnum1), true);
    }
        
        
    [Fact()]
    [Trait("Category", "Instant")]
    public void SimpleClass()
    {
        TestTranslate(typeof(TestClass), false);
    }
        
    [Fact()]
    [Trait("Category", "Instant")]
    public void Object()
    {
        TestTranslate(typeof(object), false);
    }
        
    [Fact()]
    [Trait("Category", "Instant")]
    public void NullableClassFake()
    {
        TestTranslate(typeof(NullableClassFake), false);
    }
        
        
        

    public void TestTranslate(Type type, bool canTranslate)
    {
        var canTranslateResult = ClrTypeToSqlTypeConverter.CanTranslateToSqlType(type);
        canTranslateResult.Should().Be(canTranslate);
        if(canTranslate) Console.WriteLine(ClrTypeToSqlTypeConverter.Convert(type));
    }

    public ClrTypeToSqlTypeConverterTests(ITestOutputHelper outputHelperHelper) : base(outputHelperHelper)
    {
    }
}

public enum TestEnum1
{
    Val1,
    Val2, 
    Val3
}

public class TestClass
{
    public int? NullableIdProperty { get; set; }
}

public class NullableClassFake
{
        
}