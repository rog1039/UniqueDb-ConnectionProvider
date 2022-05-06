using FluentAssertions;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public record DbTableName
{
   public string Schema { get; init; }
   public string Name   { get; init; }
    
   public DbTableName(string fullName)
   {
      var names = fullName.Split(new[] {'.'}, StringSplitOptions.RemoveEmptyEntries);
      names.Length.Should().BeGreaterOrEqualTo(2);

      var schema = names[0];
      var table  = string.Join(".", names.Skip(1));
      Schema = schema.Debracketize();
      Name   = table.Debracketize();
   }

   public DbTableName(string schema, string name)
   {
      Schema = schema;
      Name   = name;
   }
    
   public override string ToString()
   {
      return string.IsNullOrWhiteSpace(Schema)
         ? Name
         : $"[{Schema}].[{Name}]";
   }
}