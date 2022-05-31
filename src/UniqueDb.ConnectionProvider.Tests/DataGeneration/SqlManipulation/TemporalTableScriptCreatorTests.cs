using NUnit.Framework;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.DesignTimeDataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;
using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

[TestFixture]
public class IndexDmlScriptCreatorTests
{
   [Test]
   public void SimpleTest()
   {
      var annotation = IndexAnnotation.AutoName("dbo.Part", "Company");
      IndexDmlScriptCreator.GetScript(annotation).ToConsole();
      
      var annotation2 = IndexAnnotation.AutoName("dbo.Part", "Company, PartNum");
      IndexDmlScriptCreator.GetScript(annotation2).ToConsole();
   }
   [Test]
   public void WithIncludes()
   {
      var annotation = IndexAnnotation.AutoName("dbo.Part", "Company", "PartNum");
      IndexDmlScriptCreator.GetScript(annotation).ToConsole();
      
      var annotation2 = IndexAnnotation.AutoName("dbo.Part", "Company, PartNum", "IUM,PUM");
      IndexDmlScriptCreator.GetScript(annotation2).ToConsole();
   }
}

[TestFixture]
public class TemporalTableScriptCreatorTests
{
   [Test]
   public void SimpleTest()
   {
      var table = new SqlTableForTemporal()
      {
         Name   = "MySimpleTable",
         Schema = "dbo",
         Columns = new List<SqlColumnForTemporal>()
         {
            new()
            {
               Name         = "Id",
               IsPrimaryKey = true,
               SqlType      = SqlTypeFactory.Int(),
            },
         },
      };
      var script = TemporalTableScriptCreator.GetScript(table);
      script.ToConsole();
   }
   [Test]
   public void SimpleWithIndex()
   {
      var table = new SqlTableForTemporal()
      {
         Name   = "MySimpleTable",
         Schema = "dbo",
         Columns = new List<SqlColumnForTemporal>()
         {
            new()
            {
               Name         = "Id",
               IsPrimaryKey = true,
               SqlType      = SqlTypeFactory.Int(),
            },
         },
         IndexAnnotations = new List<IndexAnnotation>()
         {
            IndexAnnotation.AutoName("dbo.MySimpleTable", "Id")
         }
      };
      var script = TemporalTableScriptCreator.GetScript(table);
      script.ToConsole();
   }
}