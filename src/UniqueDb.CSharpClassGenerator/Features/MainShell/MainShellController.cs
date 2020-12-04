using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows;
using DevExpress.Mvvm;
using Newtonsoft.Json;
using Reactive.Bindings;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.DesignTimeDataGeneration;
using UniqueDb.CSharpClassGenerator.Features.DatabaseSelection;

namespace UniqueDb.CSharpClassGenerator.Features.MainShell
{
    public class MainShellController
    {
        public DatabaseSelectionController DatabaseSelectionController { get; } = new DatabaseSelectionController();

        public ReactiveProperty<string> ClassName { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<string> SqlQuery  { get; } = new ReactiveProperty<string>();

        public ReactiveProperty<string> GeneratedCSharpText { get; } = new ReactiveProperty<string>();
        public ReactiveProperty<DataTable> Datatable { get; } = new ReactiveProperty<DataTable>();
        public ReactiveProperty<decimal> DataSize { get; } = new ReactiveProperty<decimal>(0);
		
        public ReactiveProperty<string> QueryTabName { get; } = new ReactiveProperty<string>("Query Results");
        public ReactiveProperty<string> DesignTimeDataCode { get; } = new ReactiveProperty<string>();

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
            PopulateDataTable();
            GenerateCSharpClassDefinition();
            GenerateDesignTimeCode();
        }

        private void PopulateDataTable()
        {
            try
            {
                var sw = Stopwatch.StartNew();
                var dataTable = DatabaseSelectionController.GetDataTable(SqlQuery.Value);
                var elapsedTime = sw.Elapsed;
                var customJson = dataTable.ToCustomJson(Formatting.None);
                DataSize.Value = customJson.Length;
                var gzipLength = GetGzipLength(customJson);
                QueryTabName.Value =
                    $"Query Results {elapsedTime:s\\.fff}s | Rows: {dataTable.Rows.Count} | Size: {DataSize.Value / 1024:N1} KB | Compressed: {gzipLength / 1024:N1} KB";
                Datatable.Value = dataTable;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private long GetGzipLength(string customJson)
        {
            using (var compressedStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    var data = UTF8Encoding.UTF8.GetBytes(customJson);
                    zipStream.Write(data, 0, data.Length);
                    var length = compressedStream.Length;
                    zipStream.Close();
                    return length;
                }
            }
        }

        private void GenerateCSharpClassDefinition()
        {
            try
            {
                var options = new CSharpClassTextGeneratorOptions();
                options.IncludePropertyAnnotationAttributes = IncludePropertyAttributes.Value;
                var conn = DatabaseSelectionController.GetDbConnection();
                GeneratedCSharpText.Value =
                    CSharpClassGeneratorFromAdoDataReader.GenerateClass(conn, SqlQuery.Value, ClassName.Value, options);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void GenerateDesignTimeCode()
        {
            try
            {
                var json = DatabaseSelectionController.GetQueryResultsAsJson(SqlQuery.Value);
                DesignTimeDataCode.Value = DesignTimeDataCodeTemplate.CreateCode(ClassName.Value, json, GeneratedCSharpText.Value);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private bool CanExecuteQuery()
        {
            var canExecute = DatabaseSelectionController.HasDbConnectionFactory() && 
                             !string.IsNullOrWhiteSpace(ClassName.Value) && 
                             !string.IsNullOrWhiteSpace(SqlQuery.Value);
            return canExecute;
        }

        private void CopyCSharpCommand()
        {
            if(!string.IsNullOrWhiteSpace(GeneratedCSharpText.Value))
                Clipboard.SetText(GeneratedCSharpText.Value);
        }
    }
}
