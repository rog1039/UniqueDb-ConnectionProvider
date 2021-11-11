using System;
using Microsoft.Data.SqlClient;

namespace UniqueDb.ConnectionProvider;

public static class DatabaseDeleter
{
    public static void DeleteDatabase(ISqlConnectionProvider sqlConnectionProvider)
    {
        LoggerHelper.Log($"Deleting database: {sqlConnectionProvider}");
        var deleteDbSqlText = CreateSqlTextToDeleteDatabase(sqlConnectionProvider);
        var sqlConnection   = sqlConnectionProvider.ConnectionAsMaster();
        sqlConnection.Open();
        ExecuteSqlCommandToDeleteDatabase(deleteDbSqlText, sqlConnection);
        sqlConnection.Close();
        sqlConnection.Dispose();
    }

    private static string CreateSqlTextToDeleteDatabase(ISqlConnectionProvider sqlConnectionProvider)
    {
        var databaseName = sqlConnectionProvider.DatabaseName;
        var sqlCommandText =
            $"ALTER DATABASE [{databaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
            $"DROP DATABASE [{databaseName}];";
        return sqlCommandText;
    }

    private static void ExecuteSqlCommandToDeleteDatabase(string sqlCommandText, SqlConnection sqlConnection)
    {
        var sqlCommand = new SqlCommand(sqlCommandText, sqlConnection);
        try
        {
            sqlCommand.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            //Ignored...
            LoggerHelper.Log(e.ToString());
        }
    }
}