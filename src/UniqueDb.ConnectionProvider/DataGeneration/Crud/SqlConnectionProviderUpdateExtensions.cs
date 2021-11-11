using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Data.SqlClient;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud;

public static class SqlConnectionProviderUpdateExtensions
{
    public static void Update<T>(this ISqlConnectionProvider sqlConnectionProvider,
                                 T                           objectToUpdate,
                                 Expression<Func<T, object>> keyProperties      = null,
                                 string                      tableName          = null,
                                 string                      schemaName         = null,
                                 bool                        processColumnNames = true)
    {
        tableName = SqlTextFunctions.GetTableName(objectToUpdate.GetType(), tableName, schemaName);

        using (var myConnection = sqlConnectionProvider.ToSqlConnection())
        {
            using (var myCommand = new SqlCommand {Connection = myConnection})
            {
                var setClauseProperties   = SqlClrHelpers.GetRelevantPropertyInfos(objectToUpdate, null);
                var whereClauseProperties = SqlClrHelpers.GetPropertiesFromObject(objectToUpdate, keyProperties);
                SqlTextFunctions.UnUnderscoreColumnNames = processColumnNames;
                BuildOutUpdateCommand(objectToUpdate,
                                      tableName,
                                      setClauseProperties,
                                      whereClauseProperties,
                                      myCommand,
                                      processColumnNames);
                SqlTextFunctions.UnUnderscoreColumnNames = true;
                myConnection.Open();
                SqlLogger.LogSqlCommand(myCommand);
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
        }
    }

    private static void BuildOutUpdateCommand(object              objectToUpdate, string tableName,
                                              IList<PropertyInfo> setClauseProperties,
                                              IList<PropertyInfo> whereClauseProperties,
                                              SqlCommand          myCommand,
                                              bool                processColumnNames)
    {
        var setClauseColumnNames =
            setClauseProperties.Select(SqlTextFunctions.GetColumnNameFromPropertyInfo).ToList();
        var setClauseParameterNames =
            setClauseProperties.Select(SqlTextFunctions.GetSetClauseParameterName).ToList();
        var setClauseParts = Enumerable
            .Range(0, setClauseColumnNames.Count)
            .Select(index => $"{setClauseColumnNames[index]} = {setClauseParameterNames[index]}");
        var setClause = string.Join(", ", setClauseParts);

        var whereClauseColumnNames =
            whereClauseProperties.Select(SqlTextFunctions.GetColumnNameFromPropertyInfo).ToList();
        var whereClauseParameterNames =
            whereClauseProperties.Select(SqlTextFunctions.GetWhereClauseParameterName).ToList();
        var whereClauseParts = Enumerable
            .Range(0, whereClauseColumnNames.Count())
            .Select(index => $"{whereClauseColumnNames[index]} = {whereClauseParameterNames[index]}");
        var whereClause = string.Join(" AND ", whereClauseParts);

        var setClauseParameters = Enumerable
            .Range(0, setClauseProperties.Count)
            .Select(
                index =>
                    SqlTextFunctions.GetParameter(objectToUpdate, setClauseProperties[index],
                                                  setClauseParameterNames[index]))
            .ToList();
        var whereClauseParameters = Enumerable
            .Range(0, whereClauseProperties.Count)
            .Select(
                index =>
                    SqlTextFunctions.GetParameter(objectToUpdate, whereClauseProperties[index],
                                                  whereClauseParameterNames[index]))
            .ToList();


        myCommand.Parameters.AddRange(setClauseParameters.ToArray());
        myCommand.Parameters.AddRange(whereClauseParameters.ToArray());
        myCommand.CommandText = $"UPDATE {tableName} SET {setClause} WHERE {whereClause}";
    }
}