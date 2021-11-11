using FluentAssertions;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class InformationSchemaTableExtensionTests
{
    [Fact()]
    [Trait("Category", "Instant")]
    public void CreateSqlTableReferenceFromInformationSchemaTable_ShouldWork()
    {
        InformationSchemaTable informationSchemaTable = null;
        var                    sqlConnectionProvider  = new StaticSqlConnectionProvider("", "");

        "Given a InformationSchemaTable"
            ._(() =>
            {
                informationSchemaTable = InformationSchemaTableDefinitionFromJson.SampleTable().InformationSchemaTable;
            });

        "Then we should be able to create a SqlTableReference from it"
            ._(() =>
            {
                var sqlTableReference = informationSchemaTable.ToSqlTableReference(sqlConnectionProvider);
                sqlTableReference.SchemaName.Should().Be("HumanResources");
                sqlTableReference.TableName.Should().Be("Employee");
                sqlTableReference.SqlConnectionProvider.GetSqlConnectionString()
                    .Should()
                    .Be(sqlConnectionProvider.GetSqlConnectionString());
            });
    }
}

public class SqlColumnToCSharpPropertyGeneratorTests
{
    [Fact()]
    [Trait("Category", "Instant")]
    public void TestAgainstSqlDataTypes()
    {
        List<string> sqlDataTypeList = null;

        "Given a list of datatypes from SQL Server."
            ._(() =>
            {
                var connection = SqlConnectionProviders.AdventureWorksDb;
                sqlDataTypeList = connection.Query<string>("SELECT name FROM sys.Types where is_user_defined=0").ToList();
            });

        "Then we should be able to translate each of the native SQL types to a CLR type"
            ._(() =>
            {
                foreach (var sqlType in sqlDataTypeList)
                {
                    var clrType =
                        SqlTypeStringToClrTypeStringConverter.GetClrDataType(sqlType);

                }
            });
    }
}