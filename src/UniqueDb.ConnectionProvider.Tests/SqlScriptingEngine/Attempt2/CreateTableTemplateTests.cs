using NUnit.Framework;
using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlScriptingEngine.Attempt2;

namespace UniqueDb.ConnectionProvider.Tests.SqlScriptingEngine.Attempt2;

[TestFixture]
public class CreateTableTemplateTests
{
   [Test]
   public async Task TableOutput1Test()
   {
      var sut = new CreateTableTemplate() { SchemaName = "dbo", TableName = "Item" };
      sut.ColumnTemplates.Add(
         new ColumnTemplate("ItemNumber", SqlTypeFactory.NVarChar(50), false, TemporalColumnType.None));
      sut.ColumnTemplates.Add(
         new ColumnTemplate("Description", SqlTypeFactory.NVarChar(300), false, TemporalColumnType.None));
      sut.ColumnTemplates.Add(
         new ColumnTemplate("Price", SqlTypeFactory.Decimal(32, 18), true, TemporalColumnType.None));
      sut.ColumnTemplates.Add(
         new ColumnTemplate("PriceAfterDiscount", "x = y +2"));
      sut.ColumnTemplates.Add(
         new ColumnTemplate("ValidFrom", SqlTypeFactory.DateTime2(), false, TemporalColumnType.Start));
      sut.ColumnTemplates.Add(
         new ColumnTemplate("ValidTo", SqlTypeFactory.DateTime2(), false, TemporalColumnType.End));

      var result = sut.GetSql();
      result.ToConsole();
   }
}

[TestFixture]
public class ConstraintTest
{
   [Test]
   public async Task PrimaryKeyTest()
   {
      var sut = new ConstraintTemplate()
      {
         ConstraintName = "PK_dbo_OrderItem_Order_OrderItem",
         ClusterType    = ClusteredOrNot.Clustered,
         ConstraintType = ConstraintType.PrimaryKey,
         ColumnAndSorts = new()
         {
            new ColumnAndSort("OrderNumber", SortDirection.Asc),
            new ColumnAndSort("OrderLine",   SortDirection.Desc)
         },
      };

      sut.GetSql().ToConsole();
   }

   [Test]
   public async Task UniqueConstraintTest()
   {
      var sut = new ConstraintTemplate()
      {
         ConstraintName = "UQ_Item_ItemNumber",
         ClusterType    = ClusteredOrNot.NonClustered,
         ConstraintType = ConstraintType.Unique,
         ColumnAndSorts = new()
         {
            new ColumnAndSort("ItemNumber", SortDirection.Asc),
         },
      };

      sut.GetSql().ToConsole();
   }
}

[TestFixture]
public class PeriodColumnDeclarationTests
{
   [Test]
   public async Task Test1()
   {
      var sut = new PeriodColumnDeclarationTemplate()
      {
         StartColumnName = "ValidFrom",
         EndColumnName   = "ValidTo"
      };
      sut.GetSql().ToConsole();
   }
}

[TestFixture]
public class EnableSystemVersioningTemplateTests
{
   [Test]
   public async Task Test1()
   {
      var sut = new EnableSystemVersioningTemplate() { HistoryTableName = "dbo.MyTable_Archive" };
      sut.GetSql().ToConsole();
   }
}

[TestFixture]
public class AddIndexTemplateTests
{
   [Test]
   public async Task SingleColumn()
   {
      var sut = GetIndexTemplateShell();
      sut.ColumnAndSorts = new() { new("PartNumber", SortDirection.Asc) };
      sut.GetSql().ToConsole();
   }
   [Test]
   public async Task MultiColumn()
   {
      var sut = GetIndexTemplateShell();
      sut.ColumnAndSorts = new()
      {
         new("Company", SortDirection.Asc),
         new("PartNumber", SortDirection.Desc),
      };
      sut.GetSql().ToConsole();
   }

   private static AddIndexTemplate GetIndexTemplateShell()
   {
      var sut = new AddIndexTemplate()
      {
         TableName = "dbo.Parts",
         IndexName = "IX_dbo_Parts_Col1",
      };
      return sut;
   }
}