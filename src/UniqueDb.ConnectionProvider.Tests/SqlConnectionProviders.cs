namespace UniqueDb.ConnectionProvider.Tests.DataGeneration;

public static class SqlConnectionProviders
{
   private const string WS2012SqlExpress = @"WS2012SqlExp1\SqlExpress";
   
   public static readonly ISqlConnectionProvider EpicorTest905 =
      new StaticSqlConnectionProvider("EPICOR905", "EpicorTest905");

   public static readonly ISqlConnectionProvider EpicorTest905OnSql2016 =
      new StaticSqlConnectionProvider("WS2016SQL", "EpicorTest905-2016.01.09");

   public static readonly ISqlConnectionProvider PbsiCopy =
      new StaticSqlConnectionProvider(WS2012SqlExpress, "PbsiCopy");

   public static readonly ISqlConnectionProvider PbsiDatabase =
      new StaticSqlConnectionProvider(WS2012SqlExpress, "PbsiDatabase");

   public static readonly ISqlConnectionProvider AdventureWorksDb =
      new StaticSqlConnectionProvider(WS2012SqlExpress, "AdventureWorks2022");

   public static readonly ISqlConnectionProvider WideWorldImporters = 
      new StaticSqlConnectionProvider(WS2012SqlExpress, "WideWorldImporters");
}