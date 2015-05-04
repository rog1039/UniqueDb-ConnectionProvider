namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public static class LiveDbTestingSqlProvider
    {
        public static ISqlConnectionProvider AdventureWorksDb = new StaticSqlConnectionProvider("ws2012sqlexp1\\sqlexpress", "AdventureWorks2012");
    }
}