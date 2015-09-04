using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration.Crud;
using UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;
using Xbehave;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class AutofixtureInsertionTests2
    {

        [Scenario]
        public void InsertSimpleClass()
        {
            var options = new UniqueDbConnectionProviderOptions("ws2012sqlexp1\\sqlexpress", "autodisposedatabase");
            var connectionProvider = new UniqueDbConnectionProvider(options);

            "After creating a database"
                ._(() => connectionProvider.CreateDatabase());

            using (var lifecycle = connectionProvider.ToDisposable())
            {
                BddStringExtensions._("Make sure the table doesn't exist", () =>
                    {
                        var doesTableExist = connectionProvider.CheckTableExistence("dbo", "SimpleClass");
                        doesTableExist.Should().BeFalse();
                    });
                BddStringExtensions._("Create the table", () =>
                    {
                        connectionProvider.EnsureTableExists<SimpleClass>("dbo", "SimpleClass");
                    });
                BddStringExtensions._("Make sure the table does exist", () =>
                    {
                        var doesTableExist = connectionProvider.CheckTableExistence("dbo", "SimpleClass");
                        doesTableExist.Should().BeTrue();
                    });
                BddStringExtensions._("Truncate the table", () =>
                    {
                        connectionProvider.TruncateTable("dbo", "SimpleClass");
                    });

                BddStringExtensions._("Insert into the table", () =>
                {
                    connectionProvider.Insert(SimpleClass.GetSample(), "SimpleClass");
                });

                BddStringExtensions._("Delete the table", () =>
                    {
                        connectionProvider.DropTable("dbo", "SimpleClass");
                    });
                BddStringExtensions._("Make sure the table doesn't exist", () =>
                    {
                        var doesTableExist = connectionProvider.CheckTableExistence("dbo", "SimpleClass");
                        doesTableExist.Should().BeFalse();
                    });
            }
        }
    }
}
