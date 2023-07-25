using FluentAssertions;
using UniqueDb.ConnectionProvider.Converters;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlMetadata.InformationSchema;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class InformationSchemaTableExtensionTests
{
    [Fact()]
    [Trait("Category", "Instant")]
    public void CreateSqlTableReferenceFromInformationSchemaTable_ShouldWork()
    {
        SISTable sisTable = null;
        var                    sqlConnectionProvider  = new StaticSqlConnectionProvider("", "");

        "Given a InformationSchemaTable"
            ._(() =>
            {
                sisTable = InformationSchemaTableDefinitionFromJson.SampleTable().InformationSchemaTable;
            });

        "Then we should be able to create a SqlTableReference from it"
            ._(() =>
            {
                var sqlTableReference = sisTable.ToSqlTableReference(sqlConnectionProvider);
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
                        SqlToClrTypeConverter.GetClrTypeName(sqlType);

                }
            });
    }
}