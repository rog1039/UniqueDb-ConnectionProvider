using Microsoft.Data.SqlClient;


namespace UniqueDb.ConnectionProvider.DataGeneration.Crud;

public static class SqlLogger
{
    public static void LogSqlStatement(string sqlStatement)
    {
        LogSqlStatementAction?.Invoke(sqlStatement);
    }

    public static Action<string> LogSqlStatementAction { get; set; } = null;

    public static void LogSqlCommand(SqlCommand myCommand)
    {
        LogSqlStatement(myCommand.CommandText);
        LogParameters(myCommand);
    }

    public static void LogParameters(SqlCommand myCommand)
    {
        var parameterTable = myCommand.Parameters
            .Cast<SqlParameter>()
            .ToList()
            .ToStringTable(z => z.ParameterName, z => z.SqlDbType, z => z.Value);
        LogSqlStatement(parameterTable);
    }
}