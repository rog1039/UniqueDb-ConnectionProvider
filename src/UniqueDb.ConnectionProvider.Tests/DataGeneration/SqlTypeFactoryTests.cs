using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class SqlTypeFactoryTests
{
    [Fact()]
    [Trait("Category", "Instant")]
    public void TinyInt()
    {
        var type = SqlTypeFactory.TinyInt();

        type.TypeName.Should().Be("tinyint");
        type.LowerBound.Should().Be(0);
        type.UpperBound.Should().Be(255);
    }

    [Fact()]
    [Trait("Category", "Instant")]
    public void SmallInt()
    {
        var type = SqlTypeFactory.SmallInt();
            
        type.TypeName.Should().Be("smallint");
        type.LowerBound.Should().Be(-32768);
        type.UpperBound.Should().Be(32767);
    }

    [Fact()]
    [Trait("Category", "Instant")]
    public void Int()
    {
        var type = SqlTypeFactory.Int();

        type.TypeName.Should().Be("int");
        type.LowerBound.Should().Be(-2147483648);
        type.UpperBound.Should().Be(2147483647);
    }

    [Fact()]
    [Trait("Category", "Instant")]
    public void BigInt()
    {
        var type = SqlTypeFactory.BigInt();
            
        type.TypeName.Should().Be("bigint");
        type.LowerBound.Should().Be(-9223372036854775808);
        type.UpperBound.Should().Be(9223372036854775807);
    }

    [Theory]
    [MemberData(nameof(DecimalTestCases))]
    [Trait("Category", "Instant")]
    public void Decimal(int precision, int? scale, double lowerBound, double upperBound)
    {
        var type = SqlTypeFactory.Decimal(precision, scale);
            
        type.TypeName.Should().Be("decimal");
        type.LowerBound.Should().Be(lowerBound);
        type.UpperBound.Should().Be(upperBound);
    }

    [Theory]
    [MemberData(nameof(DecimalTestCases))]
    [Trait("Category", "Instant")]
    public void Numeric(int precision, int? scale, double lowerBound, double upperBound)
    {
        var type = SqlTypeFactory.Numeric(precision, scale);
            
        type.TypeName.Should().Be("decimal");
        type.LowerBound.Should().Be(lowerBound);
        type.UpperBound.Should().Be(upperBound);
    }

    public static IEnumerable<object[]> DecimalTestCases()
    {
        yield return new object[] {4, null, -9999m, 9999m};
        yield return new object[] {4, 2, -99.99m, 99.99m};
        yield return new object[] {18, 2, -9999999999999999.99m, 9999999999999999.99m};
    }

    [Fact()]
    [Trait("Category", "Instant")]
    public void Money()
    {
        var type = SqlTypeFactory.Money();
            
        type.TypeName.Should().Be("money");
        type.LowerBound.Should().Be(-922337203685477.5808);
        type.UpperBound.Should().Be(922337203685477.5807);
    }

    [Fact()]
    [Trait("Category", "Instant")]
    public void SmallMoney()
    {
        var type = SqlTypeFactory.SmallMoney();
            
        type.TypeName.Should().Be("smallmoney");
        type.LowerBound.Should().Be(-214748.3648);
        type.UpperBound.Should().Be(214748.3647);
    }
        

    [Fact()]
    [Trait("Category", "Instant")]
    public void Float()
    {
        var type = SqlTypeFactory.Float(43);
            
        type.TypeName.Should().Be("float");
        type.Mantissa.Should().Be(43);
    }
        
    [Fact()]
    [Trait("Category", "Instant")]
    public void Real()
    {
        var type = SqlTypeFactory.Real();
            
        type.TypeName.Should().Be("real");
        type.Mantissa.Should().Be(24);
    }
        
    [Fact()]
    [Trait("Category", "Instant")]
    public void SmallDateTime()
    {
        var type = SqlTypeFactory.SmallDateTime();
            
        type.TypeName.Should().Be("smalldatetime");
    }

    [Fact()]
    [Trait("Category", "Instant")]
    public void Date()
    {
        var type = SqlTypeFactory.Date();
            
        type.TypeName.Should().Be("date");
    }
        
    [Fact()]
    [Trait("Category", "Instant")]
    public void DateTime()
    {
        var type = SqlTypeFactory.DateTime();
            
        type.TypeName.Should().Be("datetime");
    }
        
    [Fact()]
    [Trait("Category", "Instant")]
    public void DateTime2()
    {
        var type = SqlTypeFactory.DateTime2(5);
            
        type.TypeName.Should().Be("datetime2");
        type.FractionalSecondsPrecision.Should().Be(5);
    }
        
    [Fact()]
    [Trait("Category", "Instant")]
    public void DateTimeOffset()
    {
        var type = SqlTypeFactory.DateTimeOffset();
            
        type.TypeName.Should().Be("datetimeoffset");
    }
        
    [Fact()]
    [Trait("Category", "Instant")]
    public void Time()
    {
        var type = SqlTypeFactory.Time();
            
        type.TypeName.Should().Be("time");
    }
}