using System.ComponentModel;
using System.Globalization;

namespace UniqueDb.ConnectionProvider.CoreTypes;

public class DbTableNameTypeConverter : TypeConverter
{
   public override object? ConvertFrom(ITypeDescriptorContext? context,
                                       CultureInfo?            culture,
                                       object                  value)
   {
      if (value is string str)
         return new DbTableName(str);

      throw new NotImplementedException();
   }

   public override object? ConvertTo(ITypeDescriptorContext? context,
                                     CultureInfo?            culture,
                                     object?                 value,
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