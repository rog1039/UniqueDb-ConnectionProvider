using System;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Dapper;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class CSharpClassCreationTests
    {
        private ISqlConnectionProvider _sqlConnectionProvider = new StaticSqlConnectionProvider("WS2012SQLEXP1\\sqlexpress", "AdventureWorks2012");
        private string _tableName = "HumanResources.Employee";

        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateClassFromSqlTable()
        {
            var sqlTableReference = new SqlTableReference(_sqlConnectionProvider, _tableName);
            var sqlTable = SqlTableFactory.Create(sqlTableReference);
            var cSharpClass = CSharpClassGenerator.GenerateClass(sqlTable);
            Console.WriteLine(cSharpClass);
        }

        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateManyRandomClasses()
        {
            string outputText = String.Empty;
            var randomSqlTableReferences = RandomTableSelector.GetRandomSqlTableReferences(_sqlConnectionProvider, 400);
            foreach (var sqlTableReference in randomSqlTableReferences)
            {
                var sqlTable = SqlTableFactory.Create(sqlTableReference);
                var cSharpClass = CSharpClassGenerator.GenerateClass(sqlTable);
                outputText += cSharpClass;
            }
            Console.WriteLine(outputText);
        }

    }
}
