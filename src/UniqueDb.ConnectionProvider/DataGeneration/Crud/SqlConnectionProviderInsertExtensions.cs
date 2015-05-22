using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud
{
    public static class SqlConnectionProviderInsertExtensions
    {
        public static void InsertWithParams(this ISqlConnectionProvider sqlConnectionProvider, object obj, string tableName = null, IEnumerable<string> columnsToIgnore = null, string schemaName = null)
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
            var columnList = string.Join(", ", propertyInfos.Select(x => x.Name));
            var sqlParameterNames = string.Join(", ", propertyInfos.Select(x => "@" + x.Name));
            var sqlParameters = propertyInfos.Select(x => GetParameter(obj, x)).ToList();

            myCommand.Parameters.AddRange(sqlParameters.ToArray());
            myCommand.CommandText = $"INSERT INTO {tableName} ({columnList}) values ({sqlParameterNames})";
        }

        private static SqlParameter GetParameter(object obj, PropertyInfo x)
        {
            var sqlParameter = new SqlParameter("@" + x.Name, x.GetValue(obj, null));
            return sqlParameter;
        }
    }
}
