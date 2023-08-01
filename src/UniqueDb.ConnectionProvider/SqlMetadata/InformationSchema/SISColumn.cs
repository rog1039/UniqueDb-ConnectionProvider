using UniqueDb.ConnectionProvider.Converters;
using UniqueDb.ConnectionProvider.CoreTypes;

namespace UniqueDb.ConnectionProvider.SqlMetadata.InformationSchema;

public class SISColumn : IEquatable<SISColumn>
{
   public int      Id                        { get; set; }
   public int SchemaDefId { get; set; }
   public int TableId { get; set; }
   
   public string  TABLE_CATALOG            { get; set; }
   public string  TABLE_SCHEMA             { get; set; }
   public string  TABLE_NAME               { get; set; }
   public string  COLUMN_NAME              { get; set; }
   public int     ORDINAL_POSITION         { get; set; }
   public string? COLUMN_DEFAULT           { get; set; }
   public string  IS_NULLABLE              { get; set; }
   public string  DATA_TYPE                { get; set; }
   public int?    CHARACTER_MAXIMUM_LENGTH { get; set; }
   public int?    CHARACTER_OCTET_LENGTH   { get; set; }
   public int?    NUMERIC_PRECISION        { get; set; }
   public int?    NUMERIC_PRECISION_RADIX  { get; set; }
   public int?    NUMERIC_SCALE            { get; set; }
   public int?    DATETIME_PRECISION       { get; set; }
   public string? CHARACTER_SET_CATALOG    { get; set; }
   public string? CHARACTER_SET_SCHEMA     { get; set; }
   public string? CHARACTER_SET_NAME       { get; set; }
   public string? COLLATION_CATALOG        { get; set; }
   public string? COLLATION_SCHEMA         { get; set; }
   public string? COLLATION_NAME           { get; set; }
   public string? DOMAIN_CATALOG           { get; set; }
   public string? DOMAIN_SCHEMA            { get; set; }
   public string? DOMAIN_NAME              { get; set; }

   public string GetSqlDataTypeStringWithNullable()
   {
      return GetSqlDataTypeString() + (IS_NULLABLE.ToUpper() == "NO" ? " NOT NULL" : " NULL");
   }

   public string GetSqlDataTypeString()
   {
      var sqlType = SqlTypeFromSISColumn.FromInformationSchemaColumn(this);
      return sqlType.ToString();
   }

   public SqlType GetSqlType() => SqlTypeFromSISColumn.FromInformationSchemaColumn(this);

   #region Equals Implementation
   public bool Equals(SISColumn? other)
   {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return string.Equals(TABLE_CATALOG, other.TABLE_CATALOG, StringComparison.OrdinalIgnoreCase) && string.Equals(TABLE_SCHEMA, other.TABLE_SCHEMA, StringComparison.OrdinalIgnoreCase) && string.Equals(TABLE_NAME, other.TABLE_NAME, StringComparison.OrdinalIgnoreCase) && string.Equals(COLUMN_NAME, other.COLUMN_NAME, StringComparison.OrdinalIgnoreCase) && ORDINAL_POSITION == other.ORDINAL_POSITION && string.Equals(COLUMN_DEFAULT, other.COLUMN_DEFAULT, StringComparison.OrdinalIgnoreCase) && string.Equals(IS_NULLABLE, other.IS_NULLABLE, StringComparison.OrdinalIgnoreCase) && string.Equals(DATA_TYPE, other.DATA_TYPE, StringComparison.OrdinalIgnoreCase) && CHARACTER_MAXIMUM_LENGTH == other.CHARACTER_MAXIMUM_LENGTH && CHARACTER_OCTET_LENGTH == other.CHARACTER_OCTET_LENGTH && NUMERIC_PRECISION == other.NUMERIC_PRECISION && NUMERIC_PRECISION_RADIX == other.NUMERIC_PRECISION_RADIX && NUMERIC_SCALE == other.NUMERIC_SCALE && DATETIME_PRECISION == other.DATETIME_PRECISION && string.Equals(CHARACTER_SET_CATALOG, other.CHARACTER_SET_CATALOG, StringComparison.OrdinalIgnoreCase) && string.Equals(CHARACTER_SET_SCHEMA, other.CHARACTER_SET_SCHEMA, StringComparison.OrdinalIgnoreCase) && string.Equals(CHARACTER_SET_NAME, other.CHARACTER_SET_NAME, StringComparison.OrdinalIgnoreCase) && string.Equals(COLLATION_CATALOG, other.COLLATION_CATALOG, StringComparison.OrdinalIgnoreCase) && string.Equals(COLLATION_SCHEMA, other.COLLATION_SCHEMA, StringComparison.OrdinalIgnoreCase) && string.Equals(COLLATION_NAME, other.COLLATION_NAME, StringComparison.OrdinalIgnoreCase) && string.Equals(DOMAIN_CATALOG, other.DOMAIN_CATALOG, StringComparison.OrdinalIgnoreCase) && string.Equals(DOMAIN_SCHEMA, other.DOMAIN_SCHEMA, StringComparison.OrdinalIgnoreCase) && string.Equals(DOMAIN_NAME, other.DOMAIN_NAME, StringComparison.OrdinalIgnoreCase);
   }

   public override bool Equals(object? obj)
   {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((SISColumn) obj);
   }

   public override int GetHashCode()
   {
      var hashCode = new HashCode();
      hashCode.Add(TABLE_CATALOG, StringComparer.OrdinalIgnoreCase);
      hashCode.Add(TABLE_SCHEMA,  StringComparer.OrdinalIgnoreCase);
      hashCode.Add(TABLE_NAME,    StringComparer.OrdinalIgnoreCase);
      hashCode.Add(COLUMN_NAME,   StringComparer.OrdinalIgnoreCase);
      hashCode.Add(ORDINAL_POSITION);
      hashCode.Add(COLUMN_DEFAULT, StringComparer.OrdinalIgnoreCase);
      hashCode.Add(IS_NULLABLE,    StringComparer.OrdinalIgnoreCase);
      hashCode.Add(DATA_TYPE,      StringComparer.OrdinalIgnoreCase);
      hashCode.Add(CHARACTER_MAXIMUM_LENGTH);
      hashCode.Add(CHARACTER_OCTET_LENGTH);
      hashCode.Add(NUMERIC_PRECISION);
      hashCode.Add(NUMERIC_PRECISION_RADIX);
      hashCode.Add(NUMERIC_SCALE);
      hashCode.Add(DATETIME_PRECISION);
      hashCode.Add(CHARACTER_SET_CATALOG, StringComparer.OrdinalIgnoreCase);
      hashCode.Add(CHARACTER_SET_SCHEMA,  StringComparer.OrdinalIgnoreCase);
      hashCode.Add(CHARACTER_SET_NAME,    StringComparer.OrdinalIgnoreCase);
      hashCode.Add(COLLATION_CATALOG,     StringComparer.OrdinalIgnoreCase);
      hashCode.Add(COLLATION_SCHEMA,      StringComparer.OrdinalIgnoreCase);
      hashCode.Add(COLLATION_NAME,        StringComparer.OrdinalIgnoreCase);
      hashCode.Add(DOMAIN_CATALOG,        StringComparer.OrdinalIgnoreCase);
      hashCode.Add(DOMAIN_SCHEMA,         StringComparer.OrdinalIgnoreCase);
      hashCode.Add(DOMAIN_NAME,           StringComparer.OrdinalIgnoreCase);
      return hashCode.ToHashCode();
   }

   public static bool operator ==(SISColumn? left, SISColumn? right)
   {
      return Equals(left, right);
   }

   public static bool operator !=(SISColumn? left, SISColumn? right)
   {
      return !Equals(left, right);
   }
   
   #endregion
}