using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using DevExpress.Mvvm;
using Microsoft.Data.SqlClient;
using Rogero.ReactiveProperty;

namespace UniqueDb.CSharpClassGenerator.Features.DatabaseSelection
{
    public class AppSettings
    {
        public static readonly TimeSpan TestConnectionTimeout = new TimeSpan(0, 0, 2);
        public static readonly TimeSpan DefaultConnectionTimeout = new TimeSpan(0, 0, 15);
    }

    public class SqlDbEntryController 
    {
        public ReactiveProperty<string> ConnectionName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> ServerName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> DatabaseName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<bool> UseIntegratedAuth { get; } = new ReactiveProperty<bool>();
        public ReactiveProperty<string> UserName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> Password { get; } = new ReactiveProperty<string>();

        public ObservableCollection<SqlRecordDto> SavedConnections => _databaseConnectionStorage.SqlRecordDtos;
        public ReactiveProperty<SqlRecordDto> SelectedConnection { get; } = new ReactiveProperty<SqlRecordDto>();

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand MakeActiveCommand { get; set; }
        public DelegateCommand TestConnectionCommand { get; set; }

        private readonly Action<Func<DbConnection>> _connectionChanged;
        private readonly DatabaseConnectionStorage _databaseConnectionStorage;

        public SqlDbEntryController(Action<Func<DbConnection>> connectionChanged, DatabaseConnectionStorage databaseConnectionStorage)
        {
            _connectionChanged = connectionChanged;
            _databaseConnectionStorage = databaseConnectionStorage;

            SaveCommand = new DelegateCommand(Save);
            DeleteCommand = new DelegateCommand(Delete);
            MakeActiveCommand = new DelegateCommand(MakeConnectionActive);
            TestConnectionCommand = new DelegateCommand(TestConnection);
            
            SelectedConnection.Subscribe(SelectedConnectionChanged);
            if (databaseConnectionStorage.SqlRecordDtos.Any())
                SelectedConnection.Value = databaseConnectionStorage.SqlRecordDtos.First();
        }

        private void Save()
        {
            var connectionDto = GetConnectionDto();
            _databaseConnectionStorage.Save(connectionDto);
            SelectedConnection.Value = connectionDto;
        }

        private SqlRecordDto GetConnectionDto()
        {
            var dto = new SqlRecordDto()
            {
                ServerName = ServerName,
                Password = Password,
                DatabaseName = DatabaseName,
                UseIntegratedAuth = UseIntegratedAuth,
                ConnectionName = ConnectionName,
                Username = UserName
            };
            return dto;
        }

        private void Delete()
        {
            var connection = _databaseConnectionStorage.SqlRecordDtos.FirstOrDefault(z => z.ConnectionName == ConnectionName.Value);
            if (connection == null) return;
            _databaseConnectionStorage.SqlRecordDtos.Remove(connection);
            _databaseConnectionStorage.SaveToDisk();
        }

        private void MakeConnectionActive()
        {
            var createConnection = new Func<DbConnection>(() => GetSqlConnection());
            _connectionChanged(createConnection);
        }

        private SqlConnection GetSqlConnection(TimeSpan connectionTimeout = default(TimeSpan))
        {
            connectionTimeout = connectionTimeout == default(TimeSpan) ? AppSettings.DefaultConnectionTimeout : connectionTimeout;
            var builder = new SqlConnectionStringBuilder();
            builder.ConnectTimeout = connectionTimeout.Seconds;
            builder.DataSource = ServerName;
            builder.InitialCatalog = DatabaseName;
            if (UseIntegratedAuth)
            {
                builder.IntegratedSecurity = true;
            }
            else
            {
                builder.IntegratedSecurity = false;
                builder.UserID = UserName;
                builder.Password = Password;
            }

            return new SqlConnection(builder.ConnectionString);
        }

        private void TestConnection()
        {
            DbConnectionTester.TestConnection(GetSqlConnection(AppSettings.TestConnectionTimeout));
        }

        private void SelectedConnectionChanged(SqlRecordDto conn)
        {
            if (conn == null) ClearInputFields();
            else SetInputFields(conn);
        }

        private void ClearInputFields()
        {
            ConnectionName.Value = string.Empty;
            ServerName.Value = string.Empty;
            DatabaseName.Value = string.Empty;
            UseIntegratedAuth.Value = false;
            UserName.Value = string.Empty;
            Password.Value = string.Empty;
        }

        private void SetInputFields(SqlRecordDto conn)
        {
            ConnectionName.Value = conn.ConnectionName;
            ServerName.Value = conn.ServerName;
            DatabaseName.Value = conn.DatabaseName;
            UseIntegratedAuth.Value = conn.UseIntegratedAuth;
            UserName.Value = conn.Username;
            Password.Value = conn.Password;
        }
    }
}
