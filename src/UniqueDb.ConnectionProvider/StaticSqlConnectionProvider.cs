using Microsoft.Data.SqlClient;

namespace UniqueDb.ConnectionProvider;

public class StaticSqlConnectionProvider : BaseSqlConnectionProvider
{
    public StaticSqlConnectionProvider(string serverName, string databaseName)
    {
        ServerName                  = serverName;
        DatabaseName                = databaseName;
        UseIntegratedAuthentication = true;
    }

    public StaticSqlConnectionProvider(string serverName, string databaseName, string userName, string password)
        : this(serverName, databaseName)
    {
        UserName                    = userName;
        Password                    = password;
        UseIntegratedAuthentication = false;
    }

    protected StaticSqlConnectionProvider(){}

    public StaticSqlConnectionProvider(string connectionString)
    {
        var connectionStringBuilder = new SqlConnectionStringBuilder(connectionString);

        ServerName   = connectionStringBuilder.DataSource;
        DatabaseName = connectionStringBuilder.InitialCatalog;

        var hasPassword = !string.IsNullOrWhiteSpace(connectionStringBuilder.Password);
        var hasUser     = !string.IsNullOrWhiteSpace(connectionStringBuilder.UserID);
        if (hasUser && hasPassword)
        {
            UserName                    = connectionStringBuilder.UserID;
            Password                    = connectionStringBuilder.Password;
            UseIntegratedAuthentication = false;
        }
        else
        {
            UseIntegratedAuthentication = true;
        }
    }

    public override SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
    {
        var builder = new SqlConnectionStringBuilder
        {
            DataSource          = ""+ServerName,
            InitialCatalog      = DatabaseName,
            IntegratedSecurity  = UseIntegratedAuthentication,
            IPAddressPreference =SqlConnectionIPAddressPreference.IPv4First,
            ConnectTimeout      = 6,
            /*
             * Added these pooling parameters because it seems in the .net 6 release we might not be pooling??
             * Update: This seems to have helped from what I can tell. I no longer have random queries taking
             * 1-7 seconds per exeuction. I did about 100 calls per endpoint by clicking around part search
             * and none were high. I think P99 was 660 ms whereas before this change P99 was 4,000 ms.
             */
            Pooling     = true,
            MinPoolSize = 10,
            // CommandTimeout = 3,
            Encrypt = false
        };

        if (UseIntegratedAuthentication) return builder;

        builder.UserID   = UserName;
        builder.Password = Password;

        return builder;
    }

    public static ISqlConnectionProvider Blank => new StaticSqlConnectionProvider("", "");

    public override string ToString()
    {
        return $"{ServerName}\\{DatabaseName} | IntegAuth: {UseIntegratedAuthentication} | User: {UserName}";
    }
}