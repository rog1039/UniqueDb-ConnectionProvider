using FluentAssertions;
using NUnit.Framework;
using UniqueDb.ConnectionProvider.CSharpGeneration;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

[TestFixture]
public class CSharpClassCreationUsingInformationSchemaTests
{
    private const string TableName = "HumanResources.Employee";

    [Fact()]
    [Trait("Category", "Integration")]
    public void CreateClassFromSqlTableReference()
    {
        var sqlTableReference = new SqlTableReference(SqlConnectionProviders.AdventureWorksDb, TableName);
        var cSharpClass       = CSharpClassGeneratorFromInformationSchema.CreateCSharpClass(sqlTableReference);
        var compileResult     = RoslynHelper.TryCompile(cSharpClass);
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
            SqlConnectionProviders.AdventureWorksDb,
            $"SELECT * from {TableName}", "Employee");
        var compileResult = RoslynHelper.TryCompile(cSharpClass);
        compileResult.IsValid().Should().BeTrue();
        Console.WriteLine(cSharpClass);
        cSharpClass.Should().Be(EmployeeCSharpClassText);
    }

    private static string EmployeeCSharpClassText => @"public class Employee
{
    [Range(-2147483648, 2147483647)]
    public int BusinessEntityID { get; set; }
    [StringLength(15)]
    public string NationalIDNumber { get; set; }
    [StringLength(256)]
    public string LoginID { get; set; }
    public SqlHierarchyId OrganizationNode { get; set; }
    [Range(-32768, 32767)]
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public Int16? OrganizationLevel { get; set; }
    [StringLength(50)]
    public string JobTitle { get; set; }
    public DateTime BirthDate { get; set; }
    [StringLength(1)]
    public string MaritalStatus { get; set; }
    [StringLength(1)]
    public string Gender { get; set; }
    public DateTime HireDate { get; set; }
    public bool SalariedFlag { get; set; }
    [Range(-32768, 32767)]
    public Int16 VacationHours { get; set; }
    [Range(-32768, 32767)]
    public Int16 SickLeaveHours { get; set; }
    public bool CurrentFlag { get; set; }
    public Guid rowguid { get; set; }
    public DateTime ModifiedDate { get; set; }
}";


    [Fact()]
    [Trait("Category", "Integration")]
    public void CreateClassForDocumentTableFromQueryTest()
    {
        var cSharpClass = CSharpClassGeneratorFromQueryViaSqlDescribeResultSet.GenerateClass(
            SqlConnectionProviders.AdventureWorksDb,
            $"SELECT * from Production.Document", "Document");
        var compileResult = RoslynHelper.TryCompile(cSharpClass);
        compileResult.IsValid().Should().BeTrue();
        Console.WriteLine(cSharpClass);
    }
}

[TestFixture]
public class CSharpClassCreationTests
{

    [Fact()]
    [Trait("Category", "Integration")]
    public void CreateClassFromSqlTableReferenceForActualUseWhenNeedingToGenerateCSharpClasses()
    {
        var sqlTableReference = new SqlTableReference(SqlConnectionProviders.PbsiCopy, "PbsiWM.MTDINV_LINE");
        var sqlTable          = SqlTableFactory.Create(sqlTableReference);
        var cSharpClass       = CSharpClassGeneratorFromSqlTable.GenerateClass(sqlTable);
        Console.WriteLine(cSharpClass);
    }

    [Fact()]
    [Trait("Category", "Integration")]
    public void CreateClassFromSqlForActualUseWhenNeedingToGenerateCSharpClasses()
    {
        var sql = @"
select im.PbsiItemNumber, im.EpicorItemNumber, i.StockWeight, i.ItemDescription, i.ItemDescription2, i.ItemDescription3
    from ItemMaps im
  join Items i on im.PbsiItemNumber = i.ItemNumber
";
        var cSharpClass = CSharpClassGeneratorFromQueryViaSqlDescribeResultSet.GenerateClass(SqlConnectionProviders.PbsiDatabase, sql, "TruckOrderRecordDto");
        Console.WriteLine(cSharpClass);
    }

    [Fact()]
    [Trait("Category", "Integration")]
    public void CreateClassFromSqlForActualUseWhenNeedingToGenerateCSharpClasses_UsingEpicorTest905()
    {
        var sql = @"
SELECT
  pb.Company
 ,pb.BinNum
 ,pb.PartNum as EpicorPartNumber
 ,CASE 
    WHEN SUM(pb.OnhandQty) IS NULL THEN 0
    ELSE  SUM(pb.OnhandQty)
  END as OnHandQty
 ,pb.DimCode
FROM PartBin pb
WHERE pb.WarehouseCode = 'DC' and pb.binnum = 'S1'
GROUP BY pb.Company
        ,PartNum
        ,pb.BinNum
        ,pb.DimCode";
            
        var cSharpClass = CSharpClassGeneratorFromAdoDataReader.GenerateClass(SqlConnectionProviders.EpicorTest905, sql, "EpicorItemInventory");
        Console.WriteLine(cSharpClass);
    }

    private const string TableName = "HumanResources.Employee";

    [Fact()]
    [Trait("Category", "Integration")]
    public void CreateClassFromSqlTableReference()
    {
        var sqlTableReference = new SqlTableReference(SqlConnectionProviders.AdventureWorksDb, TableName);
        var sqlTable          = SqlTableFactory.Create(sqlTableReference);
        var cSharpClass       = CSharpClassGeneratorFromSqlTable.GenerateClass(sqlTable);
        Console.WriteLine(cSharpClass);
    }

    [Fact()]
    [Trait("Category", "Integration")]
    public void CreateClassFromSqlQueryUsingAdoDataReader()
    {
        var query = "select * from sys.types";
        var cSharpClass = CSharpClassGeneratorFromAdoDataReader
            .GenerateClass(SqlConnectionProviders.AdventureWorksDb, query, "SysType");
        Console.WriteLine(cSharpClass);
    }

    [Fact()]
    [Trait("Category", "Integration")]
    public void CreateClassFromSqlQueryUsingAdoDataReader2()
    {
        var query = $"select * from {TableName}";
        var cSharpClass = CSharpClassGeneratorFromAdoDataReader
            .GenerateClass(SqlConnectionProviders.AdventureWorksDb, query, "Employee");
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
                classFromQuery = CSharpClassGeneratorFromQueryViaSqlDescribeResultSet
                    .GenerateClass(SqlConnectionProviders.AdventureWorksDb, query, "Employee");

                var compileResults = RoslynHelper.TryCompile(classFromQuery);
                compileResults.IsValid().Should().BeTrue();
            });
        "Given a C# class generated from SQL InformationSchema metadata"
            ._(() =>
            {
                var sqlTableReference = new SqlTableReference(SqlConnectionProviders.AdventureWorksDb, TableName);
                classFromTable = CSharpClassGeneratorFromInformationSchema.CreateCSharpClass(sqlTableReference);
                var compileResults = RoslynHelper.TryCompile(classFromTable);
                compileResults.IsValid().Should().BeTrue();
            });
        "They should produce identical output"
            ._(() =>
            {
                Console.WriteLine("From Query:\r\n" + classFromQuery);
                Console.WriteLine("From Table:\r\n" + classFromTable);
                classFromTable.Should().BeEquivalentTo(classFromQuery);
            });
    }

    [Fact()]
    [Trait("Category", "Integration")]
    public void CreateManyRandomClassesFromInformationSchema()
    {
        string outputText = String.Empty;
        var randomSqlTableReferences = RandomTableSelector.GetRandomSqlTableReferences(SqlConnectionProviders.AdventureWorksDb, 400);
        foreach (var sqlTableReference in randomSqlTableReferences)
        {
            var sqlTable    = SqlTableFactory.Create(sqlTableReference);
            var cSharpClass = CSharpClassGeneratorFromSqlTable.GenerateClass(sqlTable);
            outputText += cSharpClass;
        }
        Console.WriteLine(outputText);
    }
        
    [Fact()]
    [Trait("Category", "Integration")]
    public void CreateManyRandomClassesFromDescribeResultSet()
    {
        string outputText = String.Empty;
        var randomSqlTableReferences = RandomTableSelector.GetRandomSqlTableReferences(SqlConnectionProviders.AdventureWorksDb, 400).OrderBy(x => x.SchemaName).ThenBy(x => x.TableName);
        foreach (var sqlTableReference in randomSqlTableReferences)
        {
            var sqlTable = SqlTableFactory.Create(sqlTableReference);
            var cSharpClass = CSharpClassGeneratorFromQueryViaSqlDescribeResultSet.GenerateClass(
                sqlTableReference.SqlConnectionProvider, $"SELECT * FROM {sqlTable.Schema}.{sqlTable.Name}", sqlTable.Name);
            outputText += cSharpClass+"\r\n";
        }
        Console.WriteLine(outputText);
    }

    [Fact()]
    [Timeout(3000)]
    [Trait("Category", "Integration")]
    public void TestCompilationOfManyClasses()
    {
        int                      errorCount               = 0;
        int                      successCount             = 0;
        IList<SqlTableReference> randomSqlTableReferences = null;

        "Given a list of SqlTableReferences"
            ._(() =>
            {
                randomSqlTableReferences =
                    RandomTableSelector.GetRandomSqlTableReferences(SqlConnectionProviders.AdventureWorksDb, 400);
            });

        "Convert each table reference to a C# class and check for syntax errors using Roslyn"
            ._foreach(randomSqlTableReferences, sqlTableReference =>
            {
                var sqlTable      = SqlTableFactory.Create(sqlTableReference);
                var cSharpClass   = CSharpClassGeneratorFromSqlTable.GenerateClass(sqlTable);
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

    [TestCase()]
    public async Task BuildClassesForEntirePbsiDatabase()
    {
        /*
         * Two properties to tweak:
         *  - Do we include the ValidFrom/ValidTo temporal columns
         *  - Do we include the StringLength, Range, etc. attributes
         */
        var includeTemporalColumns = true;
        var includeAttributes      = false;
        
        var scp = new StaticSqlConnectionProvider("WS2016Sql",
            "PbsiS2STargetTemporal2");
        
        await GeneratePbsiDatabaseTypes(scp, includeAttributes, includeTemporalColumns);
    }

    private static async Task GeneratePbsiDatabaseTypes(StaticSqlConnectionProvider scp,
                                                        bool includeAttributes, bool includeTemporalColumns)
    {
        var generatorOptions = new CSharpClassTextGeneratorOptions()
            {IncludePropertyAnnotationAttributes = includeAttributes};

        var tableDefinitions = await InformationSchemaMetadataExplorer.GetInformationSchemaTableDefinitions(scp);
        foreach (var tableDefinition in tableDefinitions)
        {
            if (string.Equals(tableDefinition.InformationSchemaTable.TABLE_SCHEMA, "PbsiSf",
                    StringComparison.CurrentCultureIgnoreCase))
                continue;

            var columns = tableDefinition.InformationSchemaColumns;
            if (!includeTemporalColumns)
            {
                columns = columns.Where(z => !z.COLUMN_NAME.InsensitiveEquals("validto")
                                          && !z.COLUMN_NAME.InsensitiveEquals("validfrom")
                    )
                    .ToList();
            }

            var tableName = tableDefinition.InformationSchemaTable.TABLE_NAME
                .Replace(".", "")
                .Replace("$", "_");

            if (tableName.EndsWith("_History", StringComparison.OrdinalIgnoreCase))
                continue;

            var cSharpClass = CSharpClassGeneratorFromInformationSchema.CreateCSharpClass(
                columns,
                tableName,
                generatorOptions
            );
            Console.WriteLine(cSharpClass);
        }
    }
}