using UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

namespace UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation;

public static class IndexDmlScriptCreator
{
   public static string GetScript(IndexAnnotation indexAnnotation)
   {
      var indexName       = indexAnnotation.IndexName;
      var dbTableName     = indexAnnotation.DbTableName;
      var indexColumnList = indexAnnotation.IndexColumns.Select(z => z.ToString()).StringJoin(", ");

      var script = 
@$"
CREATE INDEX {indexName}
    ON {dbTableName}({indexColumnList})--INCLUDE--;";

      var includedColumnList = indexAnnotation.IncludedColumns.Select(z => z.ToString()).StringJoin(", ");

      if (includedColumnList.Length == 0)
         script = script.Replace("--INCLUDE--", String.Empty);
      else
         script = script
         .Replace("--INCLUDE--", $" INCLUDE ({includedColumnList})")
         .Trim();

      return script;

   }
}