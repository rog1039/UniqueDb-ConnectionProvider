using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public class InformationSchemaMetadataExplorerTests
{
    [Fact()]
    [Trait("Category", "Integration")]
    public void GetInformationSchemaTablesForAllTables()
    {
        var schemaTables = InformationSchemaMetadataExplorer
            .GetInformationSchemaTablesOnly(SqlConnectionProviders.AdventureWorksDb)
            .OrderBy(x => x.TABLE_SCHEMA)
            .ThenBy(x => x.TABLE_NAME);
        Console.WriteLine(schemaTables.ToStringTable());
    }
}