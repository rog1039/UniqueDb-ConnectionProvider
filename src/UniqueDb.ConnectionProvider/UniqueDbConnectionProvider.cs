using Microsoft.Data.SqlClient;

namespace UniqueDb.ConnectionProvider;

/// <summary>
/// A class that provides a connection string to a brand-new database.
/// </summary>
public class UniqueDbConnectionProvider : BaseSqlConnectionProvider
{
    public readonly UniqueDbConnectionProviderOptions Options;

    /// <summary>
    /// The string format used to created the database name.
    /// </summary>
    public string DatabaseNameFormatString { get; set; }

    public UniqueDbConnectionProvider(UniqueDbConnectionProviderOptions options)
    {
        Options                     = options;
        ServerName                  = Options.SqlServerName;
        DatabaseNameFormatString    = "{0}-({1})-{2:n}";
        DatabaseName                = GenerateDbName();
        UserName                    = options.UserName;
        Password                    = options.Password;
        UseIntegratedAuthentication = string.IsNullOrWhiteSpace(options.Password);
    }

    private string GenerateDbName()
    {
        var prefix    = Options.DatabaseNamePrefix;
        var timestamp = Options.IncludeTimeStamp ? DateTime.Now.ToString(Options.TimeStampFormat) : string.Empty;
        var guid      = Guid.NewGuid();

        var generatedName = string.Format(DatabaseNameFormatString,
                                          prefix,
                                          timestamp,
                                          guid);

        return generatedName;
    }

    /// <summary>
    /// Returns the SqlConnectionStringBuilder for this unique Db.
    /// </summary>
    /// <returns></returns>
    public override SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
    {
        var connString = new SqlConnectionStringBuilder();
        connString.DataSource     = Options.SqlServerName;
        connString.InitialCatalog = DatabaseName;
        if (Options.UseIntegratedSecurity)
        {
            connString.IntegratedSecurity = true;
        }
        else
        {
            connString.UserID   = Options.UserName;
            connString.Password = Options.Password.ToString();
        }
        return connString;
    }
}