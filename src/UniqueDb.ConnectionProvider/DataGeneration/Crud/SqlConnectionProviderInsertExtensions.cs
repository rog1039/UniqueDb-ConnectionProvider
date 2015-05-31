using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud
{
    public static class SqlConnectionProviderUpdateExtensions
    {
        public static void Update<T>(this ISqlConnectionProvider sqlConnectionProvider, T objectToUpdate,
            Expression<Func<T, object>> keyProperties = null, string tableName = null, string schemaName = null, bool processColumnNames = true)
        {
            tableName = SqlTextFunctions.GetTableName(objectToUpdate.GetType(), tableName, schemaName);

            using (var myConnection = sqlConnectionProvider.GetSqlConnection())
            {
                using (var myCommand = new SqlCommand() {Connection = myConnection})
                {
                    var setClauseProperties = SqlTextFunctions.GetRelevantPropertyInfos(objectToUpdate, null);
                    var whereClauseProperties = SqlTextFunctions.GetPropertiesFromObject(objectToUpdate, keyProperties);
                    SqlTextFunctions.UnUnderscoreColumnNames = processColumnNames;
                    BuildOutUpdateCommand(objectToUpdate, tableName, setClauseProperties, whereClauseProperties, myCommand, processColumnNames);
                    SqlTextFunctions.UnUnderscoreColumnNames = true;
                    myConnection.Open();
                    SqlTextFunctions.LogSqlCommand(myCommand);
                    myCommand.ExecuteNonQuery();
                    myConnection.Close();
                }
            }
        }

        private static void BuildOutUpdateCommand(object objectToUpdate, string tableName, IList<PropertyInfo> setClauseProperties, IList<PropertyInfo> whereClauseProperties, SqlCommand myCommand, bool processColumnNames)
        {
            var setClauseColumnNames = setClauseProperties.Select(SqlTextFunctions.GetColumnNameFromPropertyInfo).ToList();
            var setClauseParameterNames = setClauseProperties.Select(SqlTextFunctions.GetSetClauseParameterName).ToList();
            var setClauseParts = Enumerable
                .Range(0, setClauseColumnNames.Count)
                .Select(index => $"{setClauseColumnNames[index]} = {setClauseParameterNames[index]}");
            var setClause = string.Join(", ", setClauseParts);

            var whereClauseColumnNames = whereClauseProperties.Select(SqlTextFunctions.GetColumnNameFromPropertyInfo).ToList();
            var whereClauseParameterNames = whereClauseProperties.Select(SqlTextFunctions.GetWhereClauseParameterName).ToList();
            var whereClauseParts = Enumerable
                .Range(0, whereClauseColumnNames.Count())
                .Select(index => $"{whereClauseColumnNames[index]} = {whereClauseParameterNames[index]}");
            var whereClause = string.Join(" AND ", whereClauseParts);

            var setClauseParameters = Enumerable
                .Range(0, setClauseProperties.Count)
                .Select(index => SqlTextFunctions.GetParameter(objectToUpdate, setClauseProperties[index], setClauseParameterNames[index]))
                .ToList();
            var whereClauseParameters = Enumerable
                .Range(0, whereClauseProperties.Count)
                .Select(index => SqlTextFunctions.GetParameter(objectToUpdate, whereClauseProperties[index], whereClauseParameterNames[index]))
                .ToList();

            
            myCommand.Parameters.AddRange(setClauseParameters.ToArray());
            myCommand.Parameters.AddRange(whereClauseParameters.ToArray());
            myCommand.CommandText = $"UPDATE {tableName} SET {setClause} WHERE {whereClause}";
        }
    }

    public static class SqlConnectionProviderInsertExtensions
    {
        public static void Insert(this ISqlConnectionProvider sqlConnectionProvider, object obj, string tableName = null, string schemaName = null, IEnumerable<string> columnsToIgnore = null, bool processColumnNames = true)
        {
            tableName = SqlTextFunctions.GetTableName(obj.GetType(), tableName, schemaName);

            var propertyInfos = SqlTextFunctions.GetRelevantPropertyInfos(obj, columnsToIgnore);
            if (propertyInfos.Count == 0)
                return;

            using (var myConnection = sqlConnectionProvider.GetSqlConnection())
            {
                using (var myCommand = new SqlCommand() {Connection = myConnection})
                {
                    SqlTextFunctions.UnUnderscoreColumnNames = processColumnNames;
                    BuildOutMyCommand(obj, tableName, propertyInfos, myCommand, processColumnNames);
                    SqlTextFunctions.UnUnderscoreColumnNames = true;

                    myConnection.Open();
                    SqlTextFunctions.LogSqlCommand(myCommand);
                    
                    myCommand.ExecuteNonQuery();
                    myConnection.Close();
                }
            }
        }

        private static void BuildOutMyCommand(object obj, string tableName, IList<PropertyInfo> propertyInfos, SqlCommand myCommand, bool processColumnNames = true)
        {
            var columnList = string.Join(", ", propertyInfos.Select(SqlTextFunctions.GetColumnNameFromPropertyInfo));
            var sqlParameterNames = string.Join(", ", propertyInfos.Select(SqlTextFunctions.GetParameterName));
            var sqlParameters = propertyInfos.Select(pi => SqlTextFunctions.GetParameter(obj, pi)).ToList();

            myCommand.Parameters.AddRange(sqlParameters.ToArray());
            myCommand.CommandText = $"INSERT INTO {tableName} ({columnList}) values ({sqlParameterNames})";
        }
    }

    public static class SqlConnectionProviderQueryExtensions
    {
        public static IEnumerable<T> MyQuery<T>(this ISqlConnectionProvider sqlConnectionProvider, string whereClause = null, string tableName = null, string schemaName = null)
        {
            whereClause = whereClause ?? "1 = 1";
            var selectClause = GenerateSelectClause<T>();
            tableName = SqlTextFunctions.GetTableName(typeof (T), tableName, schemaName);
            var sqlStatement = $"SELECT {selectClause} FROM {tableName} WHERE {whereClause}";
            SqlTextFunctions.LogSqlStatement(sqlStatement);
            var results = sqlConnectionProvider.Query<T>(sqlStatement).ToList();
            return results;
        }

        private static string GenerateSelectClause<T>()
        {
            var propertyInfos = typeof (T).GetProperties();
            var columnNames = propertyInfos.Select(SqlTextFunctions.GetColumnNameFromPropertyInfo).ToList();
            var selectParts = propertyInfos.Select((info, i) => new {ColumnName = columnNames[i], Alias = info.Name}).ToList();
            var selectStrings = selectParts.Select(arg => $"{arg.ColumnName} AS {arg.Alias}");
            var selectClause = string.Join(", ", selectStrings);
            return selectClause;
        }
    }
}
