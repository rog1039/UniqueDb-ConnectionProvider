using System;
using System.Linq;
using System.Reflection;
using Microsoft.SqlServer.Types;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.Crud;
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
            var db = LiveDbTestingSqlProvider.AdventureWorksDb;
            var randomSqlTableReferences = RandomTableSelector
                .GetRandomSqlTableReferences(db, 400)
                .OrderBy(x => x.SchemaName)
                .ThenBy(x => x.TableName);
            var typesToInsert = Assembly
                .GetAssembly(typeof(BusinessEntityContact))
                .GetTypes()
                .Where(x => !x.IsAnonymousType())
                .Where(x => x.Namespace.Equals("UniqueDb.ConnectionProvider.Tests.DataGeneration.AdventureWorks"))
                .ToList();

            var fixture = new Fixture();
            fixture.Inject(new SqlGeography());
            
            foreach (var sqlTableReference in randomSqlTableReferences)
            {
                var type = typesToInsert.SingleOrDefault(t => t.Name.Equals(sqlTableReference.TableName));
                try
                {
                    var context = new SpecimenContext(fixture);
                    var value = context.Resolve(new SeededRequest(type, null));
                    db.InsertWithParams(value, sqlTableReference.TableName, sqlTableReference.SchemaName);
                    db.Delete(value, null, sqlTableReference.TableName, sqlTableReference.SchemaName);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Failed on type: {type.Name} with exception:\r\n {e}");
                }
            }

        }
    }
}
