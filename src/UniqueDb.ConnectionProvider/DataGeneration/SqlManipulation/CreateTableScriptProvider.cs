using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UniqueDb.ConnectionProvider.DataGeneration.Crud;

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
            clrProperties = OrderByColumnNumber(clrProperties);
            
            var createPropertiesSegment = clrProperties
                .Where(x => SqlClrHelpers.ShouldTranslateClrPropertyToSqlColumn(x))
                .Select(CreatePropertyInfoWithAttributes)
                .Select(PropertyInfoWithAttributeToSqlColumnDeclarationConverter.Convert)
                .Select(sqlColumnDeclaration => sqlColumnDeclaration.ToString())
                .StringJoin(",\r\n   ");


            var primaryKeySegment = GetPrimaryKeySegment(objectType, tableName);

            var createTableScript = $"CREATE TABLE {schemaName}.{tableName} " +
                                    $"(\r\n   " +
                                    $"{createPropertiesSegment}" +
                                    $"{primaryKeySegment}" +
                                    $");";

            return createTableScript;
        }

        private static string GetPrimaryKeySegment(Type objectType, string tableName)
        {
            var keyProperties = GetKeyProperties(objectType);
            if (keyProperties.Count == 0)
                return String.Empty;

            var primaryKeyConstraintName = $"PK_{tableName}";
            var keyColumnSegments = keyProperties.Select(kp => $"[{kp}] ASC");
            var keyColumnSegment = string.Join(",\r\n", keyColumnSegments);

            var completePrimaryKeySegment = $",\r\n\r\nCONSTRAINT [{primaryKeyConstraintName}] PRIMARY KEY CLUSTERED" +
                                            $"(\r\n" +
                                            $"{keyColumnSegment}" +
                                            $"\r\n)\r\n";
            return completePrimaryKeySegment;
        }

        private static IList<string> GetKeyProperties(Type objectType)
        {
            var keyProperties = objectType
                .GetProperties()
                .Where(x => x.GetCustomAttribute<KeyAttribute>() != null)
                .Select(x => x.Name)
                .ToList();
            return keyProperties;
        }

        private static List<PropertyInfo> OrderByColumnNumber(List<PropertyInfo> clrProperties)
        {
            var props = clrProperties
                .Select(
                    (info, i) =>
                        new
                        {
                            Property = info,
                            ClrPropertyOrder = i,
                            ColumnOrderAttributeOrder = GetColumnAttributeOrder(info)
                        })
                .OrderByDescending(z => z.ColumnOrderAttributeOrder.HasValue)
                .ThenBy(z => z.ColumnOrderAttributeOrder)
                .ThenBy(z => z.ClrPropertyOrder)
                .ToList();
            return props.Select(z => z.Property).ToList();
        }

        private static int? GetColumnAttributeOrder(PropertyInfo info)
        {
            var columnOrder = info.GetCustomAttribute<ColumnAttribute>()?.Order;
            return columnOrder;
        }

        private static PropertyInfoWithAttributes CreatePropertyInfoWithAttributes(PropertyInfo propertyInfo)
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
