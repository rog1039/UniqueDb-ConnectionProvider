using System.Reflection;

namespace UniqueDb.ConnectionProvider.DataGeneration
{
    public static class PropertyInfoToSqlColumnDeclarationConverter
    {
        public static SqlColumnDeclaration Convert(PropertyInfo propertyInfo)
        {
            var nullableSqlType = ClrTypeToSqlTypeConverter.Convert(propertyInfo.PropertyType);
            var name = propertyInfo.Name;
            return new SqlColumnDeclaration(name, nullableSqlType);
        }
    }
}