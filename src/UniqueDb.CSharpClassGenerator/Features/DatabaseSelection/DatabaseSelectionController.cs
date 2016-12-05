using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rogero.ReactiveProperty;

namespace UniqueDb.CSharpClassGenerator.Features.DatabaseSelection
{
    public class DatabaseSelectionController
    {
        public ReactiveProperty<Func<DbConnection>> DbConnectionFactory { get; } = new ReactiveProperty<Func<DbConnection>>();
        public SqlDbEntryController SqlDbEntryController { get; }
        public TransoftEntryController TransoftEntryController { get; }

        private readonly DatabaseConnectionStorage _databaseConnectionStorage = new DatabaseConnectionStorage();

        public DatabaseSelectionController()
        {
            _databaseConnectionStorage.Load();
            SqlDbEntryController = new SqlDbEntryController(ConnectionChanged, _databaseConnectionStorage);
            TransoftEntryController = new TransoftEntryController(ConnectionChanged, _databaseConnectionStorage);
        }

        private void ConnectionChanged(Func<DbConnection> dbConnection)
        {
            DbConnectionFactory.Value = dbConnection;
        }
    }
}
