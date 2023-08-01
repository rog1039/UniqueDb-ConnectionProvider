using NUnit.Framework;
using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Tests.DataGeneration;

namespace UniqueDb.ConnectionProvider.SqlScriptingEngine.Attempt2;

[TestFixture]
public class CreateTableTemplateTestsFromSysTable
{
   [Test]
   public async Task Test1()
   {
      var scp      = SqlConnectionProviders.WideWorldImporters;
      var tableRef = new SqlTableReference(scp, "Warehouse.StockItems");
   }
}