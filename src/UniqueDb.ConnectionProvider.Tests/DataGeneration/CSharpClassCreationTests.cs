using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Dapper;
using FluentAssertions;
using Microsoft.CodeAnalysis.Diagnostics;
using UniqueDb.ConnectionProvider.DataGeneration;
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
        public void CreateClassFromSqlQuery()
        {
            var query = "select * from sys.types";
            var columns = SqlQueryToCSharpPropertyGenerator.FromQuery(LiveDbTestingSqlProvider.AdventureWorksDb, query);
            var cSharpClass = CSharpClassGenerator.GenerateClassText("SysTypes", columns);
            Console.WriteLine(cSharpClass);
        }
        
        [Fact()]
        [Trait("Category", "Integration")]
        public void EnsureCreateClass_FromSqlTableReference_AndFromQuery_ProduceEquivalentResults()
        {
            string classFromQuery = string.Empty, classFromTable = string.Empty;
            
            "Given a C# class generated from a query"
                ._(() =>
                {
                    var query = string.Format("select * from {0}", _tableName);
                    var columns = SqlQueryToCSharpPropertyGenerator.FromQuery(LiveDbTestingSqlProvider.AdventureWorksDb, query);
                    classFromQuery = CSharpClassGenerator.GenerateClassText("Employee", columns);
                    var compileResults = RoslynHelper.TryCompile(classFromQuery);
                    compileResults.IsValid().Should().BeTrue();
                });
            "Given a C# class generated from SQL InformationSchema metadata"
                ._(() =>
                {
                    var sqlTableReference = new SqlTableReference(LiveDbTestingSqlProvider.AdventureWorksDb, _tableName);
                    var sqlTable = SqlTableFactory.Create(sqlTableReference);
                    classFromTable = CSharpClassGenerator.GenerateClass(sqlTable);
                    var compileResults = RoslynHelper.TryCompile(classFromTable);
                    compileResults.IsValid().Should().BeTrue();
                });
            "They should produce identical output"
                ._(() =>
                {
                    Console.WriteLine("From Table:\r\n" + classFromTable);
                    Console.WriteLine("From Query:\r\n" + classFromQuery);
                    classFromTable.Should().BeEquivalentTo(classFromQuery);
                });
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
