using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud
{
    public static class SqlConnectionProviderDeleteExtensions
    {
        public static void Delete<T>(this ISqlConnectionProvider sqlConnectionProvider, T obj,
            Expression<Func<T, object>> keyProperties = null, string tableName = null, string schemaName = null)
        {
            var sql = Generate(obj, keyProperties, tableName, schemaName);
            Console.WriteLine(sql);
            sqlConnectionProvider.Execute(sql);
        }

        public static string Generate<T>(T obj, Expression<Func<T, object>> keyProperties, string tableName = null, string schemaName = null)
        {
            tableName = GetTableName(obj, tableName, schemaName);
            var whereClause = GenerateWhereClauseForDeleteStatement(obj, keyProperties);

            var sqlStatement = $"DELETE FROM {tableName} WHERE {whereClause};";
            return sqlStatement;
        }

        private static string GetTableName(object obj, string tableName, string schemaName)
        {
            tableName = tableName ?? obj.GetType().Name;
            if (!string.IsNullOrWhiteSpace(schemaName)) tableName = schemaName + "." + tableName;
            return tableName;
        }

        private static string GenerateWhereClauseForDeleteStatement<T>(T obj, Expression<Func<T, object>> keyProperties)
        {
            bool useAllObjectProperties = false;
            var propertiesOfKey = new List<string>();

            if (keyProperties == null)
                useAllObjectProperties = true;
            else
                propertiesOfKey = keyProperties
                    .Body.Type
                    .GetProperties()
                    .Select(x => x.Name)
                    .ToList();

            var propertiesFromObject = obj
                .GetType()
                .GetProperties()
                .Where(x => useAllObjectProperties || propertiesOfKey.Contains(x.Name));

            var whereTests = propertiesFromObject
                .Select(x => $"{SqlConnectionProviderInsertExtensions.UnRollName(x.Name).Bracketize()} = {SqlValueEncoder.ConvertPropertyToSqlString(obj, x)}")
                .ToList();

            var whereClause = string.Join(" AND ", whereTests);
            return whereClause;
        }
    }
}