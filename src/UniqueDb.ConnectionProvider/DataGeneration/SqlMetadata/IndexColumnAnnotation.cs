namespace UniqueDb.ConnectionProvider.DataGeneration.SqlMetadata;

/// <summary>
/// A column name with its sort direction specified.
/// </summary>
public class IndexColumnAnnotation
{
   public string              ColumnName    { get; set; }
   public ColumnSortDirection SortDirection { get; set; }

   public IndexColumnAnnotation(string columnName, ColumnSortDirection sortDirection = ColumnSortDirection.Ascending)
   {
      ColumnName    = columnName;
      SortDirection = sortDirection;
   }

   public IndexColumnAnnotation(string columnName, string sort)
   {
      ColumnName    = columnName;
      SortDirection = ColumnSortDirectionParser.Parse(sort);
   }

   public override string ToString()
   {
      var sort = SortDirection == ColumnSortDirection.Ascending ? "ASC" : "DESC";
      return $"{ColumnName} {sort}";
   }
}