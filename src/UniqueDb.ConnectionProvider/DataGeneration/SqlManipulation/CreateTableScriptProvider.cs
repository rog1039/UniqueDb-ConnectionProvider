using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using UniqueDb.ConnectionProvider.DataGeneration.Crud;

namespace UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation
{
    public static class CreateTableScriptProvider
    {
        public static string GetCreateTableScript<T>(string schemaName = "dbo", string tableName = null)
        {
            var objectType = typeof(T);
            return GetCreateTableScript(objectType, schemaName, tableName);
        }

        public static string GetCreateTableScript(object obj, string schemaName = "dbo", string tableName = null)
        {
            var objectType = obj as Type ?? obj.GetType();
            return GetCreateTableScript(objectType, schemaName, tableName);
        }

        public static string GetCreateTableScriptForHierarchy(Type   rootType, 
                                                              IList<Type> subTypes,
                                                              string schemaName,
                                                              string tableName = null)
        {
            tableName = tableName ?? rootType.Name;
            var keyProperties = GetKeyProperties(rootType);
            var columnProperties = subTypes
                .SelectMany(z => z.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                .Distinct()
                .ToList();
            var tableCreationScript = GetCreateTableScript(schemaName, tableName, keyProperties, columnProperties);
            return tableCreationScript;
        }

        public static string GetCreateTableScript(Type objectType, string schemaName, string tableName = null)
        {
            tableName = tableName ?? objectType.Name;

            var columnProperties = objectType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToList();

            var keyProperties = GetKeyProperties(objectType);

            var tableCreationScript = GetCreateTableScript(schemaName, tableName, keyProperties, columnProperties);
            return tableCreationScript;
        }

        public static string GetCreateTableScript(string              schemaName,
                                                  string              tableName, 
                                                  IList<string> keyProperties,
                                                  IList<PropertyInfo> columnProperties)
        {
            columnProperties = OrderByColumnNumber(columnProperties);
            var createColumnsSegment = CreateColumnsSegment(columnProperties);
            var primaryKeySegment = CreatePrimaryKeySegment(tableName, keyProperties);
            
            var createTableScript = 
                $@"
CREATE TABLE {schemaName}.{tableName}
(
    {createColumnsSegment}
    {primaryKeySegment}
);";

            return createTableScript;
        }

        private static string CreateColumnsSegment(IList<PropertyInfo> clrProperties)
        {
            var createPropertiesSegment = clrProperties
                .Where(x => SqlClrHelpers.ShouldTranslateClrPropertyToSqlColumn(x))
                .Select(CreatePropertyInfoWithAttributes)
                .Select(PropertyInfoWithAttributeToSqlColumnDeclarationConverter.Convert)
                .Select(sqlColumnDeclaration => sqlColumnDeclaration.ToString())
                .Distinct()
                .StringJoin(",\r\n   ");
            return createPropertiesSegment;
        }

        private static string GetPrimaryKeySegment(Type objectType, string tableName)
        {
            var keyProperties = GetKeyProperties(objectType);
            if (keyProperties.Count == 0)
                return String.Empty;

            return CreatePrimaryKeySegment(tableName, keyProperties);
        }

        private static string CreatePrimaryKeySegment(string tableName, IList<string> keyProperties)
        {
            var primaryKeyConstraintName = $"PK_{tableName}";
            var keyColumnSegments        = keyProperties.Select(kp => $"[{kp}] ASC");
            var keyColumnSegment         = string.Join(",\r\n", keyColumnSegments);

            var completePrimaryKeySegment =
                $@",
CONSTRAINT [{primaryKeyConstraintName}] PRIMARY KEY CLUSTERED
(
    {keyColumnSegment}
)
";
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

        private static IList<PropertyInfo> OrderByColumnNumber(IList<PropertyInfo> clrProperties)
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
