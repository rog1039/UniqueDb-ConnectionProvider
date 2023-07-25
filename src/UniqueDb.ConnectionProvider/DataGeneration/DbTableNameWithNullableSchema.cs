using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public record DbTableNameWithNullableSchema
{
   public string? SchemaName { get; init; }
   public string  TableName  { get; init; }

   public DbTableNameWithNullableSchema(string? schemaName, string tableName)
   {
      SchemaName = schemaName.IsNullOrWhitespace() ? null : schemaName.Debracketize();
      TableName  = tableName.Debracketize();
   }

   public void Deconstruct(out string? schemaName, out string tableName)
   {
      schemaName = SchemaName;
      tableName  = TableName;
   }

   public static DbTableNameWithNullableSchema Parse(string input) => DbTableNameWithNullableSchemaParser.ParseFullTableName(input);

   public string ToStringBrackets()   => $"[{SchemaName}].[{TableName}]";
   public string ToStringNoBrackets() => $"{SchemaName}.{TableName}";
};