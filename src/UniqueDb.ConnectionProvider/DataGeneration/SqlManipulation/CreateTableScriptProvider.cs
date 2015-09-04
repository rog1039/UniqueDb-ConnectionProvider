using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation
{
    public class CreateTableScriptProvider
    {
        public static string GetCreateTableScript<T>(string schemaName = "dbo", string tableName = null)
        {
            var objectType = typeof(T);
            return GetCreateTableScript(objectType, schemaName, tableName);
        }

        public static string GetCreateTableScript(object obj, string schemaName = "dbo", string tableName = null)
        {
            var objectType = obj.GetType();
            return GetCreateTableScript(objectType, schemaName, tableName);
        }

        public static string GetCreateTableScript(Type objectType, string schemaName, string tableName = null)
        {
            tableName = tableName ?? objectType.Name;

            var clrProperties = objectType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToList();
            
            var createPropertiesSegment = clrProperties
                .Where(ShouldCreateColumnForProperty)
                .Select(CreatePropertyInfoWtihAttributes)
                .Select(PropertyInfoWithAttributeToSqlColumnDeclarationConverter.Convert)
                .Select(sqlColumnDeclaration => sqlColumnDeclaration.ToString())
                .StringJoin(",\r\n   ");

            var createTableScript = $"CREATE TABLE {schemaName}.{tableName} " +
                                    $"(\r\n   " +
                                    $"{createPropertiesSegment} " +
                                    $"\r\n);";

            return createTableScript;
        }

        private static bool ShouldCreateColumnForProperty(PropertyInfo arg)
        {
            var isString = arg.PropertyType == typeof (string);
            if (isString)
                return true;

            var implementsEnumerable = arg
                .PropertyType
                .GetInterfaces()
                .Any(interfaceType => interfaceType.Name.ToLower().Equals("ienumerable"));

            return !implementsEnumerable;
        }

        private static PropertyInfoWithAttributes CreatePropertyInfoWtihAttributes(PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo
                .GetCustomAttributes(false)
                .Where(att => att.GetType().Namespace?.Contains("System.ComponentModel.DataAnnotations") ?? false)
                .ToList();

            var result = new PropertyInfoWithAttributes(propertyInfo, attributes);
            return result;
        }
    }
}
