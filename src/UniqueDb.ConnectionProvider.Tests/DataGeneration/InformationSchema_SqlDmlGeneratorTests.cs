using Newtonsoft.Json;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata;
using UniqueDb.ConnectionProvider.SqlMetadata.InformationSchema;
using UniqueDb.ConnectionProvider.SqlScripting;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class InformationSchema_SqlDmlGeneratorTests
{

    [Fact()]
    [Trait("Category", "Integration")]
    public void RetrieveHumanResourcesEmployeeTableAsJson()
    {
        var sqlTableReference = new SqlTableReference(SqlConnectionProviders.AdventureWorksDb, "HumanResources.Employee");
        var informationSchemaTableDefinition = InformationSchemaMetadataExplorer.GetInformationSchemaTableDefinition(sqlTableReference);
        var tableDefinitionAsJson = JsonConvert.SerializeObject(informationSchemaTableDefinition);
        Console.WriteLine(tableDefinitionAsJson);
    }
        
    [Fact()]
    [Trait("Category", "Instant")]
    public void GetSampleTableDefinition()
    {
        var tableDefinition = InformationSchemaTableDefinitionFromJson.SampleTable();
        Console.WriteLine(ListExtensionMethods.MakeList(tableDefinition).ToStringTable());
    }

    [Fact()]
    [Trait("Category", "Instant")]
    public void TestSampleCreateTableScript()
    {
        var tableDefinition = InformationSchemaTableDefinitionFromJson.SampleTable();
        var script          = SISToSqlDmlCreateStatementGenerator.GenerateCreateTableScript(tableDefinition);
        Console.WriteLine(script);
    }
}