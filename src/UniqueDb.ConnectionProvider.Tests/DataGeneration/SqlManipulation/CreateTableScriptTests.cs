using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;
using Xunit;
using Xunit.Abstractions;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration.SqlManipulation;

public class SchemaTableDefCreateTableScriptTests : UnitTestBaseWithConsoleRedirection
{
    [Fact()]
    [Trait("Category", "Instant")]
    public void CreateIfTests()
    {
        var tableDef = InformationSchemaTableDefinitionFromJson.SampleTable();
        var createAnywayScript =
            SISToSqlDmlCreateStatementGenerator.GenerateCreateTableScript(
                tableDef, CreateIfExistsModification.CreateAnyway);
            
        var dropAndRecreateScript =
            SISToSqlDmlCreateStatementGenerator.GenerateCreateTableScript(
                tableDef, CreateIfExistsModification.DropAndRecreate);

        var createOnlyIfNotExistsScript =
            SISToSqlDmlCreateStatementGenerator.GenerateCreateTableScript(
                tableDef, CreateIfExistsModification.CreateIfNotExists);

        Console.WriteLine(createAnywayScript);
        Console.WriteLine(dropAndRecreateScript);
        Console.WriteLine(createOnlyIfNotExistsScript);
    } 

    public SchemaTableDefCreateTableScriptTests(ITestOutputHelper outputHelperHelper) : base(outputHelperHelper) { }
}