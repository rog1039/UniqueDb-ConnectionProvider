using System;
using System.Linq;
using System.Linq.Expressions;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud
{
    public static class DeleteSqlStatementGenerator
    {
        public static string Generate<T>(T obj, Expression<Func<T, object>> keyProperties, string tableName = null)
        {
            tableName = tableName ?? obj.GetType().Name;
            var whereClause = GenerateWhereClauseForDeleteStatement(obj, keyProperties);
            var sqlStatement = $"DELETE FROM {tableName} WHERE {whereClause};";
            return sqlStatement;
        }

        private static string GenerateWhereClauseForDeleteStatement<T>(T o, Expression<Func<T, object>> keyProperties)
        {
            var propertiesFromAnonymousObjectSelector = keyProperties.Body.Type.GetProperties().Select(x => x.Name).ToList();

            var properties = o
                .GetType()
                .GetProperties()
                .Where(x => propertiesFromAnonymousObjectSelector.Contains(x.Name));

            var whereClauseParts = properties
                .Select(x => x.Name + " = " + SqlValueEncoder.ConvertPropertyToSqlString(o, x))
                .ToList();

            var whereClause = string.Join(" AND ", whereClauseParts);

            return whereClause;
        }
    }
}