using System.Reflection;
using Microsoft.Data.SqlClient;

namespace UniqueDb.ConnectionProvider.DataGeneration.Crud;

public static class SqlConnectionProviderInsertExtensions
{
    public static void Insert(this ISqlConnectionProvider sqlConnectionProvider,
                              object                      obj,
                              string                      tableName          = null,
                              string                      schemaName         = null,
                              IEnumerable<string>         columnsToIgnore    = null,
                              bool                        processColumnNames = true,
                              bool                        useIdentityInsert  = false)
    {
        tableName = SqlTextFunctions.GetTableName(obj.GetType(), tableName, schemaName);

        var propertyInfos = SqlClrHelpers.GetRelevantPropertyInfos(obj, columnsToIgnore);
        if (propertyInfos.Count == 0)
            return;

        using (var myConnection = sqlConnectionProvider.GetSqlConnectionWithTimeout(60))
        {
            using (var myCommand = new SqlCommand {Connection = myConnection})
            {
                SqlTextFunctions.UnUnderscoreColumnNames = processColumnNames;
                BuildOutMyCommand(obj, tableName, propertyInfos, myCommand, processColumnNames);
                SqlTextFunctions.UnUnderscoreColumnNames = true;

                myConnection.Open();
                SqlLogger.LogSqlCommand(myCommand);

                // if (useIdentityInsert)
                //     myConnection.Execute("set identity_insert on;");
                myCommand.ExecuteNonQuery();
                myConnection.Close();
            }
        }
    }

    private static void BuildOutMyCommand(object     obj,       
                                          string tableName,
                                          IList<PropertyInfo> propertyInfos,
                                          SqlCommand myCommand, 
                                          bool   processColumnNames = true)
    {
        var columnList        = string.Join(", ", propertyInfos.Select(SqlTextFunctions.GetColumnNameFromPropertyInfo));
        var sqlParameterNames = string.Join(", ", propertyInfos.Select(SqlTextFunctions.GetParameterName));
        var sqlParameters     = propertyInfos.Select(pi => SqlTextFunctions.GetParameter(obj, pi)).ToList();

        myCommand.Parameters.AddRange(sqlParameters.ToArray());
        myCommand.CommandText = $"INSERT INTO {tableName} ({columnList}) values ({sqlParameterNames})";
    }
}

public static class SqlConnectionProviderQueryExtensions
{
    public static IEnumerable<T> MyQuery<T>(this ISqlConnectionProvider sqlConnectionProvider,
                                            string                      whereClause = null, 
                                            string                      tableName   = null,
                                            string                      schemaName  = null)
    {
        whereClause = whereClause ?? "1 = 1";
        var selectClause = GenerateSelectClause<T>();
        tableName = SqlTextFunctions.GetTableName(typeof (T), tableName, schemaName);
        var sqlStatement = $"SELECT {selectClause} FROM {tableName} WHERE {whereClause}";
        SqlLogger.LogSqlStatement(sqlStatement);
        var results = sqlConnectionProvider.Query<T>(sqlStatement).ToList();
        return results;
    }

    private static string GenerateSelectClause<T>()
    {
        var propertyInfos = typeof (T).GetProperties();
        var columnNames   = propertyInfos.Select(SqlTextFunctions.GetColumnNameFromPropertyInfo).ToList();
        var selectParts =
            propertyInfos.Select((info, i) => new {ColumnName = columnNames[i], Alias = info.Name}).ToList();
        var selectStrings = selectParts.Select(arg => $"{arg.ColumnName} AS {arg.Alias}");
        var selectClause  = string.Join(", ", selectStrings);
        return selectClause;
    }
}