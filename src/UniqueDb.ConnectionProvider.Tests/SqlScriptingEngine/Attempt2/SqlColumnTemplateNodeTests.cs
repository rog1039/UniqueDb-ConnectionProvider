using NUnit.Framework;
using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.CSharpGeneration.DesignTimeDataGeneration;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlScriptingEngine.Attempt2;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration.SqlManipulation.PiecewiseManipulation;

[TestFixture]
public class SqlColumnTemplateNodeTests
{
   [Test]
   public async Task ColumnOutput()
   {
      var sut = new ColumnTemplate();
      sut.ColumnNameTemplate = new ColumnNameTemplate() { ColumnName = "FirstName" };
      sut.ColumnTypeTemplate = new ColumnTypeTemplate() { SqlType    = SqlType.TextType("nvarchar", 100)};
      sut.ColumnNullabilityTemplate = new ColumnNullabilityTemplate() {AllowNull = true};
      sut.ColumnTemporalTimeTemplate = new ColumnTemporalTimeTemplate() {TemporalColumnType = TemporalColumnType.None};

      var result = sut.GetSql();
      result.ToConsole();
   }
   
   [Test]
   public async Task ColumnOutputWithTemporal()
   {
      var sut = new ColumnTemplate();
      sut.ColumnNameTemplate = new ColumnNameTemplate() { ColumnName = "ValidFrom" };
      sut.ColumnTypeTemplate = new ColumnTypeTemplate() { SqlType    = SqlTypeFactory.DateTime2()};
      sut.ColumnNullabilityTemplate = new ColumnNullabilityTemplate() {AllowNull = false};
      sut.ColumnTemporalTimeTemplate = new ColumnTemporalTimeTemplate() {TemporalColumnType = TemporalColumnType.Start};

      var result = sut.GetSql();
      result.ToConsole();
   }
}