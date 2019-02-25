using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Newtonsoft.Json;

namespace UniqueDb.CSharpClassGenerator.Features.DatabaseSelection
{
    public class DatabaseSelectionController
    {
        public SqlDbEntryController SqlDbEntryController { get; }
        public TransoftEntryController TransoftEntryController { get; }

        private Func<DbConnection> _dbConnectionFactory;
        private readonly DatabaseConnectionStorage _databaseConnectionStorage = new DatabaseConnectionStorage();

        public DatabaseSelectionController()
        {
            _databaseConnectionStorage.Load();
            SqlDbEntryController = new SqlDbEntryController(ConnectionChanged, _databaseConnectionStorage);
            TransoftEntryController = new TransoftEntryController(ConnectionChanged, _databaseConnectionStorage);
        }

        private void ConnectionChanged(Func<DbConnection> dbConnection)
        {
            _dbConnectionFactory = dbConnection;
        }

        public bool HasDbConnectionFactory() => _dbConnectionFactory != null;

        public DbConnection GetDbConnection() => _dbConnectionFactory.Invoke();

        public DataTable GetDataTable(string query)
        {
            var adapter = GetDataAdapter(query);
            var table = new DataTable();
            adapter.Fill(table);
            return table;
        }

        public string GetQueryResultsAsJson(string query, int count = 100)
        {
            var results = _dbConnectionFactory().Query(query).Take(count).ToList();
            var json = JsonConvert.SerializeObject(results);
            return json;
        }

        private DbDataAdapter GetDataAdapter(string query)
        {
            var conn = _dbConnectionFactory();
            return GetCorrectDbAdapterForConnectionType(query, conn);
        }

        private static DbDataAdapter GetCorrectDbAdapterForConnectionType(string query, DbConnection conn)
        {
            if (conn is SqlConnection)
            {
                var command = new SqlCommand(query, (SqlConnection) conn);
                var adapter = new SqlDataAdapter(command);
                return adapter;
            }
            else
            {
                var command = new OdbcCommand(query, (OdbcConnection) conn);
                var adapter = new OdbcDataAdapter(command);
                return adapter;
            }
        }
    }
}
