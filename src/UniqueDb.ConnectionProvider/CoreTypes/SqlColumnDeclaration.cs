namespace UniqueDb.ConnectionProvider.CoreTypes;

public class SqlColumnDeclaration
{
   public SqlColumnDeclaration(string name, NullableSqlType nullableSqlType)
   {
      if (string.IsNullOrWhiteSpace(name))
         throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));
      Name            = name;
      NullableSqlType = nullableSqlType ?? throw new ArgumentNullException(nameof(nullableSqlType));
   }

   public string          Name            { get; set; }
   public NullableSqlType NullableSqlType { get; set; }

   public string ToCreateDeclarationSegment()
   {
      return $"{Name} {NullableSqlType}";
   }

   public override string ToString()
   {
      return ToCreateDeclarationSegment();
   }
}