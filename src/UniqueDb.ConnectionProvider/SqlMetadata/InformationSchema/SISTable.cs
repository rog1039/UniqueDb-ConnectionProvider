using UniqueDb.ConnectionProvider.CoreTypes;

namespace UniqueDb.ConnectionProvider.SqlMetadata.InformationSchema;

public class SISTable : IEquatable<SISTable>
{
   public int Id          { get; set; }
   public int SchemaDefId { get; set; }
   
   public string TABLE_CATALOG { get; set; }
   public string TABLE_SCHEMA  { get; set; }
   public string TABLE_NAME    { get; set; }
   public string TABLE_TYPE    { get; set; }

   public DbTableName DbTableName => new(TABLE_SCHEMA, TABLE_NAME);

   #region Equals
   
   public bool Equals(SISTable? other)
   {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return string.Equals(TABLE_CATALOG, other.TABLE_CATALOG, StringComparison.OrdinalIgnoreCase) && string.Equals(TABLE_SCHEMA, other.TABLE_SCHEMA, StringComparison.OrdinalIgnoreCase) && string.Equals(TABLE_NAME, other.TABLE_NAME, StringComparison.OrdinalIgnoreCase) && string.Equals(TABLE_TYPE, other.TABLE_TYPE, StringComparison.OrdinalIgnoreCase);
   }

   public override bool Equals(object? obj)
   {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((SISTable) obj);
   }

   public override int GetHashCode()
   {
      var hashCode = new HashCode();
      hashCode.Add(TABLE_CATALOG, StringComparer.OrdinalIgnoreCase);
      hashCode.Add(TABLE_SCHEMA,  StringComparer.OrdinalIgnoreCase);
      hashCode.Add(TABLE_NAME,    StringComparer.OrdinalIgnoreCase);
      hashCode.Add(TABLE_TYPE,    StringComparer.OrdinalIgnoreCase);
      return hashCode.ToHashCode();
   }

   public static bool operator ==(SISTable? left, SISTable? right)
   {
      return Equals(left, right);
   }

   public static bool operator !=(SISTable? left, SISTable? right)
   {
      return !Equals(left, right);
   }
   
   #endregion
}