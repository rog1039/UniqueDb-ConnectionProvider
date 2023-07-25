using System.Globalization;
using Dapper;
using UniqueDb.ConnectionProvider.Infrastructure;
using UniqueDb.ConnectionProvider.Infrastructure.Extensions;
using UniqueDb.ConnectionProvider.SqlScripting;

namespace UniqueDb.ConnectionProvider;

public static class OldDatabaseDeleter
{
    /// <summary>
    /// Deletes matching databases with a lifespan of greater than 5 minutes.
    /// </summary>
    /// <param name="uniqueDbConnectionProvider"></param>
    public static void DeleteOldDatabases(UniqueDbConnectionProvider uniqueDbConnectionProvider)
    {
        DeleteOldDatabases(uniqueDbConnectionProvider, TimeSpan.FromMinutes(5));
    }

    public static void DeleteOldDatabases(UniqueDbConnectionProvider uniqueDbConnectionProvider, TimeSpan olderThan)
    {
        var oldDatabasesFromUniqueDb = GetOldDatabasesFromUniqueDb(uniqueDbConnectionProvider, olderThan);
        oldDatabasesFromUniqueDb.ForEach(database => DeleteDatabase(uniqueDbConnectionProvider, database));
    }

    public static List<string> GetOldDatabasesFromUniqueDb(UniqueDbConnectionProvider uniqueDbConnectionProvider, TimeSpan olderThan)
    {
        var connection = uniqueDbConnectionProvider.ConnectionAsMaster();
        var databases = connection
            .Query<string>("SELECT NAME FROM SYSDATABASES")
            .Where(x => x.StartsWith(uniqueDbConnectionProvider.UniqueDbOptions.DatabaseNamePrefix))
            .ToList();

        var dateTimeFormat        = uniqueDbConnectionProvider.UniqueDbOptions.TimeStampFormat;
        var databases1HourOrOlder = SelectDatabaseNamesOlderThan(databases, dateTimeFormat, olderThan);

        return databases1HourOrOlder.ToList();
    }

    public static IList<string> SelectDatabaseNamesOlderThan(List<string> databases, string dateTimeFormat, TimeSpan olderThan)
    {
        var oldDatabases = new List<string>();
        foreach (var database in databases)
        {
            try
            {
                var dateStartIndex = database.IndexOf("-(", StringComparison.Ordinal) + 2;
                var dateEndIndex   = database.IndexOf(")-", StringComparison.Ordinal) - 1;
                var dateFormat     = database.Substring(dateStartIndex, dateEndIndex - dateStartIndex + 1);
                var actualDate     = DateTime.ParseExact(dateFormat, dateTimeFormat, CultureInfo.InvariantCulture);
                var shouldSelect   = (actualDate + olderThan) < DateTime.Now;
                if (shouldSelect)
                {
                    oldDatabases.Add(database);
                }
            }
            catch (Exception e)
            {
                LoggerHelper.Log(e.ToString());
            }
        }
        return oldDatabases;
    }

    private static void DeleteDatabase(UniqueDbConnectionProvider connectionProvider, string databaseName)
    {
        var staticSqlConnectionProvider = connectionProvider.ChangeDatabase(databaseName);
        DatabaseDeleter.DeleteDatabase(staticSqlConnectionProvider);
    }

        
}