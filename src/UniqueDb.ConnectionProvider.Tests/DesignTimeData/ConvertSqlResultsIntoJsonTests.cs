using System;
using System.Linq;
using Dapper;
using DesignTimeData;
using Newtonsoft.Json;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.DesignTimeDataGeneration;
using UniqueDb.ConnectionProvider.Tests.DataGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DesignTimeData
{
    public class ConvertSqlResultsIntoJsonTests
    {
        [Fact()]
        [Trait("Category", "Integration")]
        public void TestCustomJsonSerialization()
        {
            var conn = SqlConnectionProviders.PbsiCopy;
            var results = conn.GetSqlConnection().Query("select top 5 * from item").ToList();
            var json = results.ToCustomJson(Formatting.Indented);
            Console.WriteLine(json);
        }

        [Fact()]
        [Trait("Category", "Integration")]
        public void ConvertSqlResultsToDesignTimeData()
        {
            {
                var className = "Item";
                var sqlQuery = "SELECT TOP 2 * FROM ITEM";
                var conn = SqlConnectionProviders.PbsiCopy;
                var results = conn.GetSqlConnection().Query(sqlQuery).ToList();
                var json = JsonConvert.SerializeObject(results);

                var classDeclaration = CSharpClassGeneratorFromAdoDataReader.GenerateClass(conn, sqlQuery, className);
                var designTimeCode = DesignTimeDataCodeTemplate.CreateCode("Item", json, classDeclaration);
                Console.WriteLine(designTimeCode);
            }
        }

        [Fact()]
        [Trait("Category", "Integration")]
        public void TestTemplateOutput()
        {
            var items = ItemDesignTimeData.Get();
            items.PrintStringTable();
        }
    }
}
