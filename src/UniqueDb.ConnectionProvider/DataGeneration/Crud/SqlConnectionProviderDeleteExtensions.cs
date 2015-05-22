using System;
using System.Linq;
using System.Linq.Expressions;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud
{
    public static class SqlConnectionProviderDeleteExtensions
    {
        public static void Delete<T>(this ISqlConnectionProvider sqlConnectionProvider, T obj,
            Expression<Func<T, object>> keyProperties, string tableName = null)
        {
            var sql = DeleteSqlStatementGenerator.Generate(obj, keyProperties, tableName);
            Console.WriteLine(sql);
            sqlConnectionProvider.Execute(sql);
        }

        public static string Generate<T>(T obj, Expression<Func<T, object>> keyProperties, string tableName = null, string schemaName = null)
        {
            tableName = tableName ?? obj.GetType().Name;

            var fullTableName = string.IsNullOrWhiteSpace(schemaName)
                ? tableName
                : $"{schemaName}.{tableName}";

            var whereClause = GenerateWhereClauseForDeleteStatement(obj, keyProperties);
            var sqlStatement = $"DELETE FROM {fullTableName} WHERE {whereClause};";
            return sqlStatement;
        }

        private static string GenerateWhereClauseForDeleteStatement<T>(T obj, Expression<Func<T, object>> keyProperties)
        {
            var propertiesOfKey = keyProperties
                .Body.Type
                .GetProperties()
                .Select(x => x.Name)
                .ToList();

            var propertiesFromObject = obj
                .GetType()
                .GetProperties()
                .Where(x => propertiesOfKey.Contains(x.Name));

            var whereTests = propertiesFromObject
                .Select(x => $"{x.Name} = {SqlValueEncoder.ConvertPropertyToSqlString(obj,x)}")
                .ToList();

            var whereClause = string.Join(" AND ", whereTests);
            return whereClause;
        }
    }
}