using System;
using System.Collections.Generic;
using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{

    public class CSharpClassCreationUsingInformationSchemaTests
    {
        private const string TableName = "HumanResources.Employee";

        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateClassFromSqlTableReference()
        {
            var sqlTableReference = new SqlTableReference(LiveDbTestingSqlProvider.AdventureWorksDb, TableName);
            var cSharpClass = CSharpClassGeneratorFromInformationSchema.CreateCSharpClass(sqlTableReference);
            var compileResult = RoslynHelper.TryCompile(cSharpClass);
            compileResult.IsValid().Should().BeTrue();
            Console.WriteLine(cSharpClass);
        }
    }

    public class CSharpClassCreationUsingSqlDescribeResultSetTests
    {
        private const string TableName = "HumanResources.Employee";

        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateClassFromQueryTest()
        {
            var cSharpClass = CSharpClassGeneratorFromQueryViaSqlDescribeResultSet.GenerateClass(
                LiveDbTestingSqlProvider.AdventureWorksDb,
                $"SELECT * from {TableName}", "Employee");
            var compileResult = RoslynHelper.TryCompile(cSharpClass);
            compileResult.IsValid().Should().BeTrue();
            Console.WriteLine(cSharpClass);
            cSharpClass.Should().Be(
@"public class Employee
{
    public int BusinessEntityID { get; set; }
    [MaxLength(15)]
    public string NationalIDNumber { get; set; }
    [MaxLength(256)]
    public string LoginID { get; set; }
    public SqlHierarchyId OrganizationNode { get; set; }
    public Int16? OrganizationLevel { get; set; }
    [MaxLength(50)]
    public string JobTitle { get; set; }
    public DateTime BirthDate { get; set; }
    [MaxLength(1)]
    public string MaritalStatus { get; set; }
    [MaxLength(1)]
    public string Gender { get; set; }
    public DateTime HireDate { get; set; }
    public Flag SalariedFlag { get; set; }
    public Int16 VacationHours { get; set; }
    public Int16 SickLeaveHours { get; set; }
    public Flag CurrentFlag { get; set; }
    public Guid rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }
}
");
        }
    }

    public class CSharpClassCreationTests
    {
        private const string TableName = "HumanResources.Employee";

        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateClassFromSqlTable()
        {
            var sqlTableReference = new SqlTableReference(LiveDbTestingSqlProvider.AdventureWorksDb, TableName);
            var sqlTable = SqlTableFactory.Create(sqlTableReference);
            var cSharpClass = CSharpClassGeneratorFromSqlTable.GenerateClass(sqlTable);
            Console.WriteLine(cSharpClass);
        }

        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateClassFromSqlQuery()
        {
            var query = "select * from sys.types";
            var cSharpClass = CSharpClassGeneratorFromAdoDataReader
                .GenerateClass(LiveDbTestingSqlProvider.AdventureWorksDb, query, "SysType");
            Console.WriteLine(cSharpClass);
        }

        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateClassFromSqlQuery2()
        {
            var query = $"select * from {TableName}";
            var cSharpClass = CSharpClassGeneratorFromAdoDataReader
                .GenerateClass(LiveDbTestingSqlProvider.AdventureWorksDb, query, "Employee");
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
                    classFromQuery = CSharpClassGeneratorFromAdoDataReader
                        .GenerateClass(LiveDbTestingSqlProvider.AdventureWorksDb, query, "Employee");

                    var compileResults = RoslynHelper.TryCompile(classFromQuery);
                    compileResults.IsValid().Should().BeTrue();
                });
            "Given a C# class generated from SQL InformationSchema metadata"
                ._(() =>
                {
                    var sqlTableReference = new SqlTableReference(LiveDbTestingSqlProvider.AdventureWorksDb, TableName);
                    classFromTable = CSharpClassGeneratorFromInformationSchema.CreateCSharpClass(sqlTableReference);
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
                var cSharpClass = CSharpClassGeneratorFromSqlTable.GenerateClass(sqlTable);
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
                    var cSharpClass = CSharpClassGeneratorFromSqlTable.GenerateClass(sqlTable);
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
