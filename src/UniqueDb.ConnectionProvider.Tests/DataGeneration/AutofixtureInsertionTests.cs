using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Types;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.Crud;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;
using UniqueDb.ConnectionProvider.Tests.DataGeneration.AdventureWorks;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class AutofixtureInsertionTests
    {
        [Fact()]
        [Trait("Category", "Integration")]
        public void InsertManyRandomRowsIntoManyRandomTables_UsingAutoFixture()
        {
            string outputText = String.Empty;
            var randomSqlTableReferences = RandomTableSelector.GetRandomSqlTableReferences(LiveDbTestingSqlProvider.AdventureWorksDb, 400);
            var types = Assembly
                .GetAssembly(typeof(BusinessEntityContact))
                .GetTypes()
                .Where(x => !x.IsAnonymousType())
                .Where(x => x.Namespace.Equals("UniqueDb.ConnectionProvider.Tests.DataGeneration.AdventureWorks"))
                .ToList();
            var fixture = new Fixture();
            fixture.Inject<SqlGeography>(new SqlGeography());

            var db = LiveDbTestingSqlProvider.AdventureWorksDb;
            
            foreach (var sqlTableReference in randomSqlTableReferences)
            {
                var type = types.SingleOrDefault(t => t.Name.Equals(sqlTableReference.TableName));
                try
                {
                    var context = new SpecimenContext(fixture);
                    var value = context.Resolve(new SeededRequest(type, null));
                    db.InsertWithParams(value, sqlTableReference.TableName, schemaName: sqlTableReference.SchemaName);
                    Console.WriteLine($"Success on type {type.Name}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed on type: {type.Name} with exception:\r\n {e}");
                }
            }

        }
    }
}
