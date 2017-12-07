using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.Crud;
using UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;
using Xbehave;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class InsertionTests
    {

        [Scenario]
        public void InsertSimpleClass()
        {
            var options = new UniqueDbConnectionProviderOptions("ws2012sqlexp1\\sqlexpress", "autodisposedatabase");
            var connectionProvider = new UniqueDbConnectionProvider(options);

            "After creating a database"
                ._(() => connectionProvider.CreateDatabase());

            using (var lifecycle = connectionProvider.ToSelfDeletingDisposable())
            {
                BddStringExtensions._("Create the table", () =>
                {
                    connectionProvider.EnsureTableExists<SimpleClass>("dbo", "SimpleClass");
                });
                BddStringExtensions._("Insert into the table", () =>
                {
                    connectionProvider.Insert(SimpleClass.GetSample(), "SimpleClass");
                });
            }
        }

        [Scenario]
        public void SlowBulkInsertSimpleClass()
        {
            var options = new UniqueDbConnectionProviderOptions("ws2012sqlexp1\\sqlexpress", "autodisposedatabase");
            var connectionProvider = new UniqueDbConnectionProvider(options);

            "After creating a database"
                ._(() => connectionProvider.CreateDatabase());

            using (var lifecycle = connectionProvider.ToSelfDeletingDisposable())
            {
                BddStringExtensions._("Create the table", () =>
                {
                    connectionProvider.EnsureTableExists<SimpleClass>("dbo", "SimpleClass");
                });
                BddStringExtensions._("Insert into the table", () =>
                {
                    var listOfSimpleClass = Enumerable.Range(0, 9999).Select(i => SimpleClass.GetSample()).ToList();
                    foreach (var simpleClass in listOfSimpleClass)
                    {
                        connectionProvider.Insert(simpleClass, "SimpleClass");
                    }
                });
            }
        }

        [Scenario]
        public void SqlBulkInsertSimpleClass()
        {
            var options = new UniqueDbConnectionProviderOptions("ws2012sqlexp1\\sqlexpress", "autodisposedatabase");
            var connectionProvider = new UniqueDbConnectionProvider(options);

            "After creating a database"
                ._(() => connectionProvider.CreateDatabase());

            using (var lifecycle = connectionProvider.ToSelfDeletingDisposable())
            {
                BddStringExtensions._("Create the table", () =>
                {
                    connectionProvider.EnsureTableExists<SimpleClass>("dbo", "SimpleClass");
                });
                BddStringExtensions._("Insert into the table", () =>
                {
                    var listOfSimpleClass = Enumerable.Range(0, 9999).Select(i => SimpleClass.GetSample()).ToList();
                    connectionProvider.BulkInsert(listOfSimpleClass, "SimpleClass");
                });
            }
        }
    }
}
