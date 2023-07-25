namespace UniqueDb.ConnectionProvider;

/// <summary>
/// A class that provides a connection string to a brand-new database.
/// </summary>
public class UniqueDbConnectionProvider : BaseSqlConnectionProvider
{
   public readonly UniqueDbConnectionProviderOptions UniqueDbOptions;

   /// <summary>
   /// The string format used to created the database name.
   /// </summary>
   public string DatabaseNameFormatString { get; } = "{0}-({1})-{2}";

   public UniqueDbConnectionProvider(UniqueDbConnectionProviderOptions uniqueDbOptions)
   {
      UniqueDbOptions             = uniqueDbOptions;
      ServerName                  = UniqueDbOptions.SqlServerName;
      DatabaseName                = GenerateDbName(uniqueDbOptions, DatabaseNameFormatString);
      UserName                    = uniqueDbOptions.UserName;
      Password                    = uniqueDbOptions.Password;
      UseIntegratedAuthentication = string.IsNullOrWhiteSpace(uniqueDbOptions.Password);
   }

   private static string GenerateDbName(UniqueDbConnectionProviderOptions options,
                                        string                            databaseNameFormatString)
   {
      var prefix    = options.DatabaseNamePrefix;
      var timestamp = options.IncludeTimeStamp ? DateTime.Now.ToString(options.TimeStampFormat) : string.Empty;
      var nanoId    = Nanoid.Nanoid.Generate(size: 5);

      var generatedName = string.Format(databaseNameFormatString,
                                        prefix,
                                        timestamp,
                                        nanoId);
      return generatedName;
   }
}