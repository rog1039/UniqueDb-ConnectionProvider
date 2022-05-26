using System.ComponentModel;
using System.Globalization;
using FluentAssertions;

namespace UniqueDb.ConnectionProvider.DataGeneration;

public class DbTableNameTypeConverter : TypeConverter
{
   public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
   {
      if (value is string str)
         return new DbTableName(str);

      throw new NotImplementedException();
   }

   public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value,
                                     Type                    destinationType)
   {
      if (value is DbTableName dbTableName && destinationType == typeof(string))
         return dbTableName.ToString();

      throw new NotImplementedException();
   }

   public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
   {
      return sourceType == typeof(string);
   }

   public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
   {
      return destinationType == typeof(DbTableName);
   }
}

[TypeConverter(typeof(DbTableNameTypeConverter))]
public record DbTableName
{
   public string Schema { get; init; }
   public string Name   { get; init; }

   public DbTableName(string fullName)
   {
      var tableName = DbTableNameWithNullableSchemaParser.ParseFullTableName(fullName);

      Schema = tableName?.SchemaName ??
               throw new InvalidOperationException($"Provided fullname, {fullName}, did not contain a schema.");
      Name = tableName.TableName;
   }

   public DbTableName(string schema, string name)
   {
      Schema = schema.Debracketize();
      Name   = name.Debracketize();
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
}