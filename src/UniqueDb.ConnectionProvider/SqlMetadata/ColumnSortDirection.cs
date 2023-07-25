namespace UniqueDb.ConnectionProvider.SqlMetadata;

public enum ColumnSortDirection
{
   Ascending,
   Descending,
}

public static class ColumnSortDirectionParser
{
   public static ColumnSortDirection Parse(string input)
   {
      input = input.Trim().ToLower();
      return input switch
             {
                "asc"  => ColumnSortDirection.Ascending,
                "desc" => ColumnSortDirection.Descending,
                _      => throw new InvalidDataException($"Input must be either asc or desc, was {input}")
             };
   }
}
