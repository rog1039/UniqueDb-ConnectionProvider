using System;
using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;
using Xbehave;

namespace UniqueDb.ConnectionProvider.Tests
{
    public class EnsureTableExistsTests
    {
        [Scenario]
        public void ShouldDispose()
        {
            var options = new UniqueDbConnectionProviderOptions("ws2012sqlexp1\\sqlexpress", "autodisposedatabase");
            var connectionProvider = new UniqueDbConnectionProvider(options);

            "After creating a database"
                ._(() => connectionProvider.CreateDatabase());

            using (var lifecycle = connectionProvider.ToDisposable())
            {
                "Make sure the table doesn't exist"
                    ._(() =>
                    {
                        var doesTableExist = connectionProvider.CheckTableExistence("dbo", "SimpleClass");
                        doesTableExist.Should().BeFalse();
                    });
                "Create the table"
                    ._(() =>
                    {
                        connectionProvider.EnsureTableExists<SimpleClass>("dbo", "SimpleClass");
                    });
                "Make sure the table does exist"
                    ._(() =>
                    {
                        var doesTableExist = connectionProvider.CheckTableExistence("dbo", "SimpleClass");
                        doesTableExist.Should().BeTrue();
                    });
                "Truncate the table"
                    ._(() =>
                    {
                        connectionProvider.TruncateTable("dbo", "SimpleClass");
                    });

                "Delete the table"
                    ._(() =>
                    {
                        connectionProvider.DropTable("dbo", "SimpleClass");
                    });
                "Make sure the table doesn't exist"
                    ._(() =>
                    {
                        var doesTableExist = connectionProvider.CheckTableExistence("dbo", "SimpleClass");
                        doesTableExist.Should().BeFalse();
                    });
            }
        }
    }

    public class SimpleClass
    {
        public int Id { get; set; }
        public string SomeString { get; set; }
        public DateTime SomeDate { get; set; }
        public bool SomeBoolean { get; set; }
        public SimpleEnum SimpleEnum { get; set; }

        public static SimpleClass GetSample()
        {
            var simpleClass = new SimpleClass()
            {
                SimpleEnum = SimpleEnum.ValueOne,
                Id = 10,
                SomeString = "alskdfjals",
                SomeDate = new DateTime(1,1,1),
                SomeBoolean = false
            };
            return simpleClass;
        }

    }

    public enum SimpleEnum
    {
        ValueOne,
        ValueTwo,
        ValueThree
    }
}