using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud
{
    public static class SqlConnectionProviderInsertExtensions
    {
        public static void InsertWithParams(this ISqlConnectionProvider sqlConnectionProvider, object obj, string tableName = null, string schemaName = null, IEnumerable<string> columnsToIgnore = null)
        {
            tableName = GetTableName(obj, tableName, schemaName);

            var propertyInfos = GetRelevantPropertyInfos(obj, columnsToIgnore);
            if (propertyInfos.Count == 0)
                return;

            using (var myConnection = sqlConnectionProvider.GetSqlConnection())
            {
                using (var myCommand = new SqlCommand() {Connection = myConnection})
                {
                    BuildOutMyCommand(obj, tableName, propertyInfos, myCommand);

                    myConnection.Open();
                    Console.WriteLine(myCommand.CommandText);
                    myCommand.ExecuteNonQuery();
                    myConnection.Close();
                }
            }
        }

        private static string GetTableName(object obj, string tableName, string schemaName)
        {
            tableName = tableName ?? obj.GetType().Name;
            if (!string.IsNullOrWhiteSpace(schemaName)) tableName = schemaName + "." + tableName;
            return tableName;
        }

        private static List<PropertyInfo> GetRelevantPropertyInfos(object obj, IEnumerable<string> columnsToIgnore)
        {
            columnsToIgnore = columnsToIgnore ?? new List<string>();
            var propertyInfos = obj.GetType()
                .GetProperties()
                .Where(x => !columnsToIgnore.Contains(x.Name))
                .Where(x => x.CustomAttributes.All(a => a.AttributeType != typeof(DatabaseGeneratedAttribute)))
                .ToList();
            return propertyInfos;
        }

        private static void BuildOutMyCommand(object obj, string tableName, List<PropertyInfo> propertyInfos, SqlCommand myCommand)
        {
            var columnList = string.Join(", ", propertyInfos.Select(pi => pi.Name.Replace("_", " ").Bracketize()));
            var sqlParameterNames = string.Join(", ", propertyInfos.Select(pi => GetParameterName(pi.Name)));
            var sqlParameters = propertyInfos.Select(pi => GetParameter(obj, pi)).ToList();

            myCommand.Parameters.AddRange(sqlParameters.ToArray());
            myCommand.CommandText = $"INSERT INTO {tableName} ({columnList}) values ({sqlParameterNames})";
        }

        public static string GetParameterName(string name) => "@" + name;
        public static string UnRollName(string name) => name.Replace("_", " ");


        private static SqlParameter GetParameter(object obj, PropertyInfo pi)
        {
            var sqlParameter = new SqlParameter(GetParameterName(pi.Name), pi.GetValue(obj, null));
            if (!SqlTypes.IsClrTypeASqlSystemType(pi.PropertyType))
            {
                Console.WriteLine($"*****{obj.GetType()} - {pi} - {pi.Name} ");
                sqlParameter.UdtTypeName = pi.PropertyType.Name;
            }
            return sqlParameter;
        }
    }
}
