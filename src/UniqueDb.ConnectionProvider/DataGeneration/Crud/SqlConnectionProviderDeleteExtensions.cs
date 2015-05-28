using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud
{
    public static class SqlConnectionProviderDeleteExtensions
    {
        public static void Delete<T>(this ISqlConnectionProvider sqlConnectionProvider, T objToDelete,
            Expression<Func<T, object>> keyProperties = null, string tableName = null, string schemaName = null)
        {
            tableName = SqlTextFunctions.GetTableName(objToDelete.GetType(), tableName, schemaName);
            
            using (var myConnection = sqlConnectionProvider.GetSqlConnection())
            {
                using (var myCommand = new SqlCommand() { Connection = myConnection })
                {
                    BuildOutMyCommand(objToDelete, keyProperties, tableName, myCommand);

                    myConnection.Open();
                    SqlTextFunctions.LogSqlStatement(myCommand.CommandText);
                    myCommand.ExecuteNonQuery();
                    myConnection.Close();
                }
            }
        }

        private static void BuildOutMyCommand<T>(T objToDelete, Expression<Func<T, object>> keyProperties, string tableName, SqlCommand myCommand)
        {
            var whereClauseProperties = SqlTextFunctions.GetPropertiesFromObject(objToDelete, keyProperties);
            var whereClauseColumnNames = whereClauseProperties.Select(SqlTextFunctions.GetColumnNameFromPropertyInfo).ToList();
            var whereClauseParameterNames = whereClauseProperties.Select(SqlTextFunctions.GetParameterName).ToList();
            var whereClauseParts = Enumerable
                .Range(0, whereClauseColumnNames.Count)
                .Select(index => $"{whereClauseColumnNames[index]} = {whereClauseParameterNames[index]}");
            var whereClause = string.Join(" AND ", whereClauseParts);

            var whereClauseParameters = Enumerable
                .Range(0, whereClauseProperties.Count)
                .Select(
                    index => SqlTextFunctions.GetParameter(objToDelete, whereClauseProperties[index], whereClauseParameterNames[index]))
                .ToList();
            myCommand.Parameters.AddRange(whereClauseParameters.ToArray());
            myCommand.CommandText = $"DELETE FROM {tableName} WHERE {whereClause}";
        }
    }
}