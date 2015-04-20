using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniqueDb.ConnectionProvider
{
    public class UniqueDbDisposable : IDisposable
    {
        private readonly ISqlConnectionProvider _dbConnectionProvider;

        public UniqueDbDisposable(ISqlConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public void Dispose()
        {
            DeleteDatabase();
        }

        private void DeleteDatabase()
        {
            var deleteDbSqlText = CreateSqlTextToDeleteDatabase();
            var sqlConnection = CreateSqlConnectionToMasterDatabase();
            sqlConnection.Open();
            ExecuteSqlCommandToDeleteDatabase(deleteDbSqlText, sqlConnection);
            sqlConnection.Close();
            sqlConnection.Dispose();
        }

        private string CreateSqlTextToDeleteDatabase()
        {
            String sqlCommandText = string.Format(
                "ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; " +
                "DROP DATABASE [{0}];",
                _dbConnectionProvider.DatabaseName);
            return sqlCommandText;
        }

        private SqlConnection CreateSqlConnectionToMasterDatabase()
        {
            var sqlConnectionBuilder = _dbConnectionProvider.GetSqlConnectionStringBuilder();
            sqlConnectionBuilder.InitialCatalog = "master";
            var sqlConnection = new SqlConnection(sqlConnectionBuilder.ConnectionString);
            return sqlConnection;
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

    public static class UniqueDbDisposableExtensionMethods
    {
        public static UniqueDbDisposable ToDisposable(this ISqlConnectionProvider dbConnectionProvider)
        {
            var disposable = new UniqueDbDisposable(dbConnectionProvider);
            return disposable;
        }
    }
}
