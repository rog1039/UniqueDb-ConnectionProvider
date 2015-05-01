using System;
using System.CodeDom.Compiler;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Microsoft.CodeAnalysis.Diagnostics;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class CSharpClassCreationTests
    {
        private string _tableName = "HumanResources.Employee";

        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateClassFromSqlTable()
        {
            var sqlTableReference = new SqlTableReference(LiveDbTestingSqlProvider.AdventureWorksDb, _tableName);
            var sqlTable = SqlTableFactory.Create(sqlTableReference);
            var cSharpClass = CSharpClassGenerator.GenerateClass(sqlTable);
            Console.WriteLine(cSharpClass);
        }

        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateManyRandomClasses()
        {
            string outputText = String.Empty;
            var randomSqlTableReferences = RandomTableSelector.GetRandomSqlTableReferences(LiveDbTestingSqlProvider.AdventureWorksDb, 400);
            foreach (var sqlTableReference in randomSqlTableReferences)
            {
                var sqlTable = SqlTableFactory.Create(sqlTableReference);
                var cSharpClass = CSharpClassGenerator.GenerateClass(sqlTable);
                outputText += cSharpClass;
            }
            Console.WriteLine(outputText);
        }
        
        [Fact()]
        [Timeout(3000)]
        [Trait("Category", "Integration")]
        public void TestCompilationOfManyClasses()
        {
            int errorCount = 0;
            int successCount = 0;
            var randomSqlTableReferences = RandomTableSelector.GetRandomSqlTableReferences(LiveDbTestingSqlProvider.AdventureWorksDb, 400);
            foreach (var sqlTableReference in randomSqlTableReferences)
            {
                var sqlTable = SqlTableFactory.Create(sqlTableReference);
                var cSharpClass = CSharpClassGenerator.GenerateClass(sqlTable);
                var compileResult = RoslynHelper.TryCompile(cSharpClass);
                if (!compileResult.IsValid())
                {
                    errorCount++;
                    Console.WriteLine("Error found in the following:\r\n"+cSharpClass);
                }
                else
                {
                    successCount++;
                }
            }
            Console.WriteLine("Successes: {0}", successCount);
            Console.WriteLine("Failures:  {0}", errorCount);
            
            errorCount.Should().Be(0);
        }
    }
}
