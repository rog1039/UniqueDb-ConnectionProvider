using UniqueDb.ConnectionProvider.CoreTypes;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider.SqlMetadata;

/// <summary>
/// Allows us to describe an index we wish to apply to a table.
/// </summary>
public class IndexAnnotation
{
   public DbTableName                  DbTableName     { get; set; }
   public string                       IndexName       { get; set; }
   public IList<IndexColumnAnnotation> IndexColumns    { get; set; }
   public IList<string>                IncludedColumns { get; set; }

   public IndexAnnotation(string tableName, string indexName, IList<IndexColumnAnnotation> indexColumns, IList<string> includedColumns)
   {
      DbTableName     = tableName;
      IndexName       = indexName;
      IndexColumns    = indexColumns;
      IncludedColumns = includedColumns;
   }

   /// <summary>
   /// Creates an IndexAnnotation for the table on the specified columns with specified included columns.
   /// Uses generic index naming convention.
   /// </summary>
   /// <param name="tableName" />
   /// <param name="indexColumnsString" />
   /// <param name="includedColumnsText" />
   /// <returns />
   public static IndexAnnotation AutoName(string tableName, string indexColumnsString, string? includedColumnsText = null)
   {
      includedColumnsText ??= string.Empty;

      List<IndexColumnAnnotation> indexColumns    = ParseIndexColumns(indexColumnsString);
      List<string>                includedColumns = includedColumnsText.SplitOn(',').ToList();

      var indexName = GetIndexName("", indexColumns);

      return new IndexAnnotation(
         tableName,
         indexName,
         indexColumns,
         includedColumns);
   }

   /// <summary>
   /// Turns a string of index column annotation into a List<IndexColumnAnnotation>.
   /// E.g. "partnum, lotnum asc, trandate desc"
   /// </summary>
   /// <param name="indexColumnsString"></param>
   /// <returns></returns>
   /// <exception cref="InvalidDataException"></exception>
   private static List<IndexColumnAnnotation> ParseIndexColumns(string indexColumnsString)
   {
      List<IndexColumnAnnotation> indexColumns = new List<IndexColumnAnnotation>();

      var indexedParts = indexColumnsString.SplitOn(',');
      foreach (var indexedPart in indexedParts)
      {
         var pieces = indexedPart.SplitOn(' ');
         if (pieces.Length == 1) indexColumns.Add(new IndexColumnAnnotation(pieces[0]));
         if (pieces.Length == 2) indexColumns.Add(new IndexColumnAnnotation(pieces[0], pieces[1]));
         if (pieces.Length > 2)
            throw new InvalidDataException($"Should only have column name and optionally asc/desc. Text was {indexedPart}");
      }

      return indexColumns;
   }

   private static string GetIndexName(string nameSuffix, IEnumerable<IndexColumnAnnotation> indexColumns)
   {
      return GetIndexName(nameSuffix, indexColumns.Select(z => z.ColumnName));
   }

   /// <summary>
   /// Gets the index name as IX_ followed by all the columns. Does not include columns if the name suffix is non-empty.
   /// </summary>
   /// <param name="nameSuffix"></param>
   /// <param name="columns"></param>
   /// <returns></returns>
   private static string GetIndexName(string nameSuffix, IEnumerable<string> columns)
   {
      /*
       * Index names only need to be unique per table, so we don't need the table name from the index name.
       */
      if (nameSuffix.IsNullOrWhitespace())
      {
         nameSuffix = $"{columns.StringJoin("_")}";
      }

      return $"IX_{nameSuffix}";
   }

   public override string ToString()
   {
      return
         $"{DbTableName,-20} - {IndexName,-20} - {IndexColumns.Select(z => z.ToString()).StringJoin(", ")} - {IncludedColumns.StringJoin(",")}";
   }

//    private static void ParseViaRegex()
//    {
//       /*
//          * Sample text to match.
//  asc
//  desc
//  partnum
//  partnum,
//  partnum asc
//  partnum Desc
//  partnum, lotnum
//  partnum asc, lotnum
//  partnum, lotnum desc
//  partnum desc, lotnum asc
//  partnum, lotnum, company
//  partnum, lotnum dEsc, company asc
//
//          */
//       var exp = new Regex("(?<Column>\\w+)(?:(?:\\s+(?<Sort>asc|desc)))?", RegexOptions.IgnoreCase);
//    }
}