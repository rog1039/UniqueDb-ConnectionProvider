using System;
using System.Data.SqlClient;

namespace UniqueDb.ConnectionProvider
{
    public static class DatabaseDeleter
    {
        public static void DeleteDatabase(ISqlConnectionProvider sqlConnectionProvider)
        {
            var deleteDbSqlText = CreateSqlTextToDeleteDatabase(sqlConnectionProvider);
            var sqlConnection = sqlConnectionProvider.ConnectionAsMaster();
            sqlConnection.Open();
            ExecuteSqlCommandToDeleteDatabase(deleteDbSqlText, sqlConnection);
            sqlConnection.Close();
            sqlConnection.Dispose();
        }

        private static string CreateSqlTextToDeleteDatabase(ISqlConnectionProvider sqlConnectionProvider)
        {
            String sqlCommandText = string.Format(
                "ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
                "DROP DATABASE [{0}];", sqlConnectionProvider.DatabaseName);
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
                Console.WriteLine(e);
            }
        }
    }
}