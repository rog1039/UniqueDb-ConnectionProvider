using System;
using System.Collections.Generic;
using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class CSharpClassCreationTests
    {
        private const string TableName = "HumanResources.Employee";

        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateClassFromSqlTable()
        {
            var sqlTableReference = new SqlTableReference(LiveDbTestingSqlProvider.AdventureWorksDb, TableName);
            var sqlTable = SqlTableFactory.Create(sqlTableReference);
            var cSharpClass = CSharpClassGenerator.GenerateClass(sqlTable);
            Console.WriteLine(cSharpClass);
        }

        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateClassFromSqlQuery()
        {
            var query = "select * from sys.types";
            var cSharpClass = LiveDbTestingSqlProvider.AdventureWorksDb.GenerateClass(query, "SysType");
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
                    var query = string.Format("select * from {0}", TableName);
                    var columns = SqlQueryToCSharpPropertyGenerator.FromQuery(LiveDbTestingSqlProvider.AdventureWorksDb, query);
                    classFromQuery = CSharpClassGenerator.GenerateClassText("Employee", columns);
                    var compileResults = RoslynHelper.TryCompile(classFromQuery);
                    compileResults.IsValid().Should().BeTrue();
                });
            "Given a C# class generated from SQL InformationSchema metadata"
                ._(() =>
                {
                    var sqlTableReference = new SqlTableReference(LiveDbTestingSqlProvider.AdventureWorksDb, TableName);
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
            IList<SqlTableReference> randomSqlTableReferences = null;

            "Given a list of SqlTableReferences"
                ._(() =>
                {
                    randomSqlTableReferences =
                        RandomTableSelector.GetRandomSqlTableReferences(LiveDbTestingSqlProvider.AdventureWorksDb, 400);
                });

            "Convert each table reference to a C# class and check for syntax errors using Roslyn"
                ._foreach(randomSqlTableReferences, sqlTableReference =>
                {
                    var sqlTable = SqlTableFactory.Create(sqlTableReference);
                    var cSharpClass = CSharpClassGenerator.GenerateClass(sqlTable);
                    var compileResult = RoslynHelper.TryCompile(cSharpClass);

                    if (compileResult.IsValid())
                        successCount++;
                    else
                    {
                        errorCount++;
                        Console.WriteLine("Error found in the following:\r\n" + cSharpClass);
                    }
                });

            "Then print out testing results"
                ._(() =>
                {
                    Console.WriteLine("Successes: {0}", successCount);
                    Console.WriteLine("Failures:  {0}", errorCount);
                    errorCount.Should().Be(0);
                });
        }
    }
}
