namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public static class SqlConnectionProviders
{
    public static ISqlConnectionProvider EpicorTest905 = new StaticSqlConnectionProvider("EPICOR905", "EpicorTest905");
    public static ISqlConnectionProvider EpicorTest905OnSql2016 = new StaticSqlConnectionProvider("WS2016SQL", "EpicorTest905-2016.01.09");
    public static ISqlConnectionProvider PbsiCopy = new StaticSqlConnectionProvider("WS2012sqlexp1\\sqlexpress", "PbsiCopy");
    public static ISqlConnectionProvider PbsiDatabase = new StaticSqlConnectionProvider("WS2012sqlexp1\\sqlexpress", "PbsiDatabase");
    public static ISqlConnectionProvider AdventureWorksDb = new StaticSqlConnectionProvider("ws2012sqlexp1\\sqlexpress", "AdventureWorks2012");
    
    public static ISqlConnectionProvider AdventureWorks = new StaticSqlConnectionProvider(
        "WS2012SqlExp1\\SqlExpress", 
        "AdventureWorks2022");
    public static ISqlConnectionProvider WideWorldImporters = new StaticSqlConnectionProvider(
        "WS2012SqlExp1\\SqlExpress", 
        "WideWorldImporters");
}