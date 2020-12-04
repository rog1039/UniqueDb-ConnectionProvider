using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using DevExpress.Mvvm;
using Reactive.Bindings;

namespace UniqueDb.CSharpClassGenerator.Features.DatabaseSelection
{
    public class TransoftEntryController
    {
        public ReactiveProperty<string> ConnectionName   { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ConnectionString { get; } = new ReactiveProperty<string>();

        public ObservableCollection<OdbcDsnDto> SavedConnections => _databaseConnectionStorage.OdbcDsnDtos;
        public ReactiveProperty<OdbcDsnDto> SelectedConnection { get; } = new ReactiveProperty<OdbcDsnDto>();

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand MakeActiveCommand { get; set; }
        public DelegateCommand TestConnectionCommand { get; set; }

        private readonly Action<Func<DbConnection>> _connectionChanged;
        private readonly DatabaseConnectionStorage _databaseConnectionStorage;

        public TransoftEntryController(Action<Func<DbConnection>> connectionChanged, DatabaseConnectionStorage databaseConnectionStorage)
        {
            _connectionChanged = connectionChanged;
            _databaseConnectionStorage = databaseConnectionStorage;

            SaveCommand = new DelegateCommand(Save);
            DeleteCommand = new DelegateCommand(Delete);
            MakeActiveCommand = new DelegateCommand(MakeConnectionActive);
            TestConnectionCommand = new DelegateCommand(TestConnection);

            SelectedConnection.Subscribe(SelectedConnectionChanged);
            if (databaseConnectionStorage.OdbcDsnDtos.Any())
                SelectedConnection.Value = databaseConnectionStorage.OdbcDsnDtos.First();
        }

        private void Save()
        {
            var connectionDto = GetConnectionDto();
            _databaseConnectionStorage.Save(connectionDto);
            SelectedConnection.Value = connectionDto;
        }

        private OdbcDsnDto GetConnectionDto() => new OdbcDsnDto() {ConnectionName = ConnectionName.Value, DsnName = ConnectionString.Value};

        private void Delete()
        {
            var connection = _databaseConnectionStorage.OdbcDsnDtos.FirstOrDefault(z => z.ConnectionName == ConnectionName.Value);
            if (connection == null) return;
            _databaseConnectionStorage.OdbcDsnDtos.Remove(connection);
            _databaseConnectionStorage.SaveToDisk();
        }

        private void MakeConnectionActive()
        {
            var createOdbcConnection = new Func<DbConnection>(() => GetConnection(AppSettings.DefaultConnectionTimeout));
            _connectionChanged(createOdbcConnection);
        }

        private void TestConnection()
        {
            DbConnectionTester.TestConnection(GetConnection(AppSettings.TestConnectionTimeout));
        }

        private DbConnection GetConnection(TimeSpan testConnectionTimeout)
        {
            var odbcBuilder = new OdbcConnectionStringBuilder {Dsn = ConnectionString.Value};
            var connectionString = odbcBuilder.ConnectionString;
            var odbcConn = new OdbcConnection(connectionString) {};
            return odbcConn;
        }

        private void SelectedConnectionChanged(OdbcDsnDto conn)
        {
            if (conn == null) ClearInputFields();
            else SetInputFields(conn);

        }

        private void ClearInputFields()
        {
            ConnectionName.Value = string.Empty;
            ConnectionString.Value = string.Empty;
        }

        private void SetInputFields(OdbcDsnDto conn)
        {
            ConnectionName.Value = conn.ConnectionName;
            ConnectionString.Value = conn.DsnName;
        }
    }
}
