using System.Collections.ObjectModel;
using System.Windows;
using DevExpress.Mvvm;
using Reactive.Bindings;
using UniqueDb.ConnectionProvider;
using UniqueDb.ConnectionProvider.CSharpGeneration;

namespace UniqueDb.CSharpClassGenerator.Features.CodeGen;

public class CodeGenController
{
    public ReactiveProperty<SqlConnectionHolder> SelectedSqlConnection { get; } = new ReactiveProperty<SqlConnectionHolder>();
    public ObservableCollection<SqlConnectionHolder> SqlConnections { get; } = new ObservableCollection<SqlConnectionHolder>(SqlConnectionHolders.All);
    public ReactiveProperty<string> ClassName { get; } = new ReactiveProperty<string>();
    public ReactiveProperty<string> SqlQuery { get; } = new ReactiveProperty<string>();
    public ReactiveProperty<string> GeneratedCode { get; } = new ReactiveProperty<string>();

    public DelegateCommand GenerateCodeCommand { get; private set; }
    public DelegateCommand CopyCodeCommand     { get; private set; }

    public CodeGenController()
    {
        CopyCodeCommand             = new DelegateCommand(CopyCode,     CanCopyCode);
        GenerateCodeCommand         = new DelegateCommand(GenerateCode, CanGenerateCode);
        SelectedSqlConnection.Value = SqlConnections.Skip(1).First();
    }

    private void CopyCode()
    {
        Clipboard.SetText(GeneratedCode.Value);
    }

    private bool CanCopyCode()
    {
        return !string.IsNullOrWhiteSpace(GeneratedCode.Value);
    }

    private void GenerateCode()
    {
        try
        {
            var sqlConnectionProvider = SelectedSqlConnection.Value.SqlConnectionProvider;
            var cSharpClass = CSharpClassGeneratorFromAdoDataReader.GenerateClass(sqlConnectionProvider, SqlQuery.Value, ClassName.Value);
            GeneratedCode.Value = cSharpClass;
        }
        catch (Exception e)
        {
            MessageBox.Show(string.Join(Environment.NewLine, e.Message, e.StackTrace, e.ToString()));
        }
    }

    private bool CanGenerateCode()
    {
        return SelectedSqlConnection.Value != null 
            && !string.IsNullOrWhiteSpace(ClassName.Value) 
            && !string.IsNullOrWhiteSpace(SqlQuery.Value);
    }
}

public class SqlConnectionHolder
{
    public string                 Name                  { get; set; }
    public ISqlConnectionProvider SqlConnectionProvider { get; set; }
}

public class SqlConnectionHolders
{

    public static SqlConnectionHolder Epicor905Test = new SqlConnectionHolder()
    {
        Name                  = "Epicor905 Test",
        SqlConnectionProvider = new StaticSqlConnectionProvider("epicor905", "EpicorTest905")
    };
    public static SqlConnectionHolder PbsiDatabase = new SqlConnectionHolder()
    {
        Name                  = "PBSI Database",
        SqlConnectionProvider = new StaticSqlConnectionProvider("ws2012sqlexp1\\sqlexpress", "PbsiDatabase")
    };
    public static SqlConnectionHolder PbsiCopy = new SqlConnectionHolder()
    {
        Name                  = "PBSI Copy",
        SqlConnectionProvider = new StaticSqlConnectionProvider("ws2012sqlexp1\\sqlexpress", "PbsiCopy")
    };
    public static SqlConnectionHolder PbsiSyncMetadata = new SqlConnectionHolder()
    {
        Name                  = "PBSI Sync Metadata",
        SqlConnectionProvider = new StaticSqlConnectionProvider("ws2016sql", "PbsiSyncMetadata")
    };
    public static IList<SqlConnectionHolder> All = new List<SqlConnectionHolder>()
    {
        Epicor905Test, PbsiDatabase, PbsiCopy, PbsiSyncMetadata
    };
}