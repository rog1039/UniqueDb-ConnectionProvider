using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UniqueDb.ConnectionProvider.DataGeneration.SqlManipulation
{
    public class CreateTableScriptProvider
    {
        public static string GetCreateTableScript<T>(string tableName = null)
        {
            var objectType = typeof(T);
            return GetCreateTableScript(objectType, tableName);
        }

        public static string GetCreateTableScript(object obj, string tableName = null)
        {
            var objectType = obj.GetType();
            return GetCreateTableScript(objectType, tableName);
        }

        public static string GetCreateTableScript(Type objectType, string tableName = null)
        {
            tableName = tableName ?? objectType.Name;

            var clrProperties = objectType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToList();

            var createPropertiesSegment = clrProperties
                .Select(PropertyInfoToSqlColumnDeclarationConverter.Convert)
                .Select(sqlColumnDeclaration => sqlColumnDeclaration.ToString())
                .StringJoin(",\r\n   ");

            var createTableScript = $"CREATE TABLE {tableName} " +
                                    $"(\r\n   " +
                                    $"{createPropertiesSegment} " +
                                    $"\r\n);";

            return createTableScript;
        }
    }
}
