using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using DevExpress.Mvvm;
using Rogero.ReactiveProperty;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;
using UniqueDb.CSharpClassGenerator.Features.DatabaseSelection;

namespace UniqueDb.CSharpClassGenerator.Features.MainShell
{
    public class MainShellController
    {
        public DatabaseSelectionController DatabaseSelectionController { get; } = new DatabaseSelectionController();

        public ReactiveProperty<string> ClassName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> SqlQuery { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> GeneratedCSharpText { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<DataTable> Datatable { get; } = new ReactiveProperty<DataTable>();
        public ReactiveProperty<string> QueryTabName { get; } = new ReactiveProperty<string>("Query Results");
		
        public ReactiveProperty<bool> IncludePropertyAttributes { get; } = new ReactiveProperty<bool>(false);
		
		
        public DelegateCommand ExecuteQueryCommand { get; }
        public DelegateCommand CopyCSharpClassCommand { get; }

        public MainShellController()
        {
            ExecuteQueryCommand = new DelegateCommand(ExecuteQuery, CanExecuteQuery);
            CopyCSharpClassCommand = new DelegateCommand(CopyCSharpCommand);
        }

        private void ExecuteQuery()
        {
            try
            {
                var sw = Stopwatch.StartNew();
                var dataTable = DatabaseSelectionController.GetDataTable(SqlQuery);
                QueryTabName.Value = "Query Results " + sw.Elapsed.ToString("s\\.fff") + "s | Rows: " + dataTable.Rows.Count;
                Datatable.Value = dataTable;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }

            try
            {
                var options = new CSharpClassTextGeneratorOptions();
                options.IncludePropertyAnnotationAttributes = IncludePropertyAttributes.Value;
                var conn = DatabaseSelectionController.GetDbConnection();
                GeneratedCSharpText.Value = CSharpClassGeneratorFromAdoDataReader.GenerateClass(conn, SqlQuery.Value, ClassName.Value, options);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private bool CanExecuteQuery()
        {
            var canExecute = DatabaseSelectionController.HasDbConnectionFactory() && 
                             !string.IsNullOrWhiteSpace(ClassName) && 
                             !string.IsNullOrWhiteSpace(SqlQuery);
            return canExecute;
        }

        private void CopyCSharpCommand()
        {
            if(!string.IsNullOrWhiteSpace(GeneratedCSharpText.Value))
                Clipboard.SetText(GeneratedCSharpText.Value);
        }
    }
}
