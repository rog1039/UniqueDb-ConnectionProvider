using System.ComponentModel;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider.CoreTypes;

[TypeConverter(typeof(DbTableNameTypeConverter))]
public record DbTableName
{
   public string Schema { get; init; }
   public string Name   { get; init; }

   public DbTableName(string fullName)
   {
      var tableName = DbTableNameWithNullableSchemaParser.ParseFullTableName(fullName);

      Schema = tableName.SchemaName ??
               throw new InvalidOperationException($"Provided fullname, {fullName}, did not contain a schema.");
      Name = tableName.TableName;
   }

   public DbTableName(string schema, string name)
   {
      Schema = schema.Debracketize();
      Name   = name.Debracketize();
   }

   public string ToStringQuoted()
   {
      return string.IsNullOrWhiteSpace(Schema)
         ? $"\"{Name}\""
         : $"\"[{Schema}]\".\"[{Name}]\"";
   }
   
   public override string ToString()
   {
      return string.IsNullOrWhiteSpace(Schema)
         ? Name
         : $"[{Schema}].[{Name}]";
   }
   
   public static implicit operator DbTableName(string input)
   {
      return new DbTableName(input);
   }
   public static implicit operator DbTableName(SqlTableReference sqlTableReference)
   {
      return new DbTableName(sqlTableReference.SchemaName, sqlTableReference.TableName);
   }

   public virtual bool Equals(DbTableName? other)
   {
      if (other is null) return false;
      if (ReferenceEquals(this, other)) return true;
      return string.Equals(Schema, other.Schema, StringComparison.OrdinalIgnoreCase) && string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
   }

   public override int GetHashCode()
   {
      var hashCode = new HashCode();
      hashCode.Add(Schema, StringComparer.OrdinalIgnoreCase);
      hashCode.Add(Name,   StringComparer.OrdinalIgnoreCase);
      return hashCode.ToHashCode();
   }
}