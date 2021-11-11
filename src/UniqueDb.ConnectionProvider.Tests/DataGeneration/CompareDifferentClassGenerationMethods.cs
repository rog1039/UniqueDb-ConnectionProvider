using System;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class CompareDifferentClassGenerationMethods
{
    private static readonly string SchemaName        = "dbo";
    private static readonly string TableName         = "PoDetail";
    private static readonly string PoDetailClassName = "PoDetail";

    private static string SqlSelectFromPoDetail => $"SELECT * FROM {SchemaName}.{TableName}";
    private static ISqlConnectionProvider SqlConnectionProvider => SqlConnectionProviders.EpicorTest905OnSql2016;
    private static SqlTableReference SqlTableReference => new SqlTableReference(SqlConnectionProvider, SchemaName, TableName);

    [Fact()]
    [Trait("Category", "Integration")]
    public void FromAdoDataReader()
    {
        var text = CSharpClassGeneratorFromAdoDataReader.GenerateClass(
            SqlConnectionProvider,
            SqlSelectFromPoDetail,
            PoDetailClassName,
            CSharpClassTextGeneratorOptions.Default);
        Console.WriteLine(text);
    }

    [Fact()]
    [Trait("Category", "Integration")]
    public void FromDescribeResultSet()
    {
        var text = CSharpClassGeneratorFromQueryViaSqlDescribeResultSet.GenerateClass(
            SqlConnectionProvider,
            SqlSelectFromPoDetail,
            PoDetailClassName);
        Console.WriteLine(text);
    }

    [Fact()]
    [Trait("Category", "Integration")]
    public void FromInformationSchema()
    {
        var text = CSharpClassGeneratorFromInformationSchema.CreateCSharpClass(SqlTableReference);
        Console.WriteLine(text);
    }
}