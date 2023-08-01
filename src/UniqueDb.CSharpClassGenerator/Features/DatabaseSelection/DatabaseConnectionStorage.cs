using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using Newtonsoft.Json;
using UniqueDb.ConnectionProvider;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;

namespace UniqueDb.CSharpClassGenerator.Features.DatabaseSelection;

public class DatabaseConnectionStorage
{
    public ObservableCollection<OdbcDsnDto>   OdbcDsnDtos   { get; set; } = new ObservableCollection<OdbcDsnDto>();
    public ObservableCollection<SqlRecordDto> SqlRecordDtos { get; set; } = new ObservableCollection<SqlRecordDto>();

    public void Save(SqlRecordDto dto)
    {
        var existing = SqlRecordDtos.SingleOrDefault(z => z.ConnectionName == dto.ConnectionName);
        if (existing != null)
        {
            SqlRecordDtos.Remove(existing);
        }
        SqlRecordDtos.Add(dto);
        SaveToDisk();
    }

    public void Save(OdbcDsnDto dto)
    {
        var existing = OdbcDsnDtos.SingleOrDefault(z => z.ConnectionName == dto.ConnectionName);
        if (existing != null)
        {
            OdbcDsnDtos.Remove(existing);
        }
        OdbcDsnDtos.Add(dto);
        SaveToDisk();
    }

    public void SaveToDisk()
    {
        try
        {
            var json = JsonConvert.SerializeObject(this);
            File.WriteAllText(SettingsPersistencePathProvider.GetFilePath(), json);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
    }

    public void Load()
    {
        try
        {
            var json = File.ReadAllText(SettingsPersistencePathProvider.GetFilePath());
            var obj  = JsonConvert.DeserializeObject<DatabaseConnectionStorage>(json);
            this.SqlRecordDtos.Clear();
            this.OdbcDsnDtos.Clear();
            SqlRecordDtos.AddRange(obj.SqlRecordDtos.OrderBy(x => x.ServerName).ThenBy(x => x.DatabaseName));
            OdbcDsnDtos.AddRange(obj.OdbcDsnDtos);
        }
        catch (Exception e)
        {
            MessageBox.Show(e.ToString());
        }
    }
}

public static class SettingsPersistencePathProvider
{
    private static string _connectionsFileName = "connections.json";

    public static string GetFilePath()
    {
        return GetNetworkPath();
        //return GetLocalPath();
    }

    public static string GetNetworkPath()
    {
        var docsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        var filePath = Path.Combine(docsPath, _connectionsFileName);
        filePath = @"\\woebermustard.com\files\Redirection\paulrogero\My Documents\connections.json";
        return filePath;
    }

    public static string GetLocalPath()
    {
        var docsPath = ".";
        var filePath = $"{docsPath}\\{_connectionsFileName}";
        return filePath;
    }
}