namespace UniqueDb.ConnectionProvider.CoreTypes;

/// <summary>
/// Just a copy of values from the DescribeResultSet. Further analysis can narrow this type into
/// the more defined SqlType.
/// </summary>
public class SqlTypeSpecification
{
   public string TypeName                   { get; set; }
   public int?   NumericPrecision           { get; set; }
   public int?   NumericScale               { get; set; }
   public int?   MaxCharacterLength         { get; set; }
   public string MaxCharacterText           { get; set; }
   public int?   FractionalSecondsPrecision { get; set; }
}