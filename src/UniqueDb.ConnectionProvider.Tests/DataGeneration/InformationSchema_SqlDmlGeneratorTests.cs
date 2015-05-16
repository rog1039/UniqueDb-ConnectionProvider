using System;
using Newtonsoft.Json;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class InformationSchema_SqlDmlGeneratorTests
    {

        [Fact()]
        [Trait("Category", "Integration")]
        public void RetrieveHumanResourcesEmployeeTableAsJson()
        {
            var sqlTableReference = new SqlTableReference(LiveDbTestingSqlProvider.AdventureWorksDb, "HumanResources.Employee");
            var informationSchemaTableDefinition = InformationSchemaMetadataExplorer.GetInformationSchemaTableDefinition(sqlTableReference);
            var tableDefinitionAsJson = JsonConvert.SerializeObject(informationSchemaTableDefinition);
            Console.WriteLine(tableDefinitionAsJson);
        }
        
        [Fact()]
        [Trait("Category", "Instant")]
        public void GetSampleTableDefinition()
        {
            var tableDefinition = InformationSchemaTableDefinitionFromJson.SampleTable();
            Console.WriteLine(tableDefinition.MakeList().ToStringTable());
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public void TestSampleCreateTableScript()
        {
            var tableDefinition = InformationSchemaTableDefinitionFromJson.SampleTable();
            var script = SqlDmlCreateTableFromInformationSchemaGenerator.GenerateCreateTableScript(tableDefinition);
            Console.WriteLine(script);
        }
    }
}