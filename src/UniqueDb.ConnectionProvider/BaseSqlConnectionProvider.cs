using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace UniqueDb.ConnectionProvider;

public abstract class BaseSqlConnectionProvider : ISqlConnectionProvider, IEquatable<BaseSqlConnectionProvider>
{
    public string DatabaseName { get; protected set; }
    public string ServerName   { get; protected set; }

    public bool   UseIntegratedAuthentication { get; protected set; }
    public string UserName                    { get; protected set; }
    public string Password                    { get; protected set; }

    public virtual string GetSqlConnectionString() => GetSqlConnectionStringBuilder().ConnectionString;

    public virtual DbConnection GetSqlConnection()
    {
        var builder = GetSqlConnectionStringBuilder();
        builder.ConnectTimeout = Int32.MaxValue;
        var sqlConnection = new SqlConnection(builder.ConnectionString);
        // var profiledDbConnection = new ProfiledDbConnection(sqlConnection, MiniProfiler.Current);
        return sqlConnection;
    }

    public virtual SqlConnection GetSqlConnectionWithTimeout(int timeout)
    {
        var builder = GetSqlConnectionStringBuilder();
        builder.ConnectTimeout = timeout;
        return new SqlConnection(builder.ConnectionString);
    }

    public string JustInstanceName => GetInstanceName();

    private string GetInstanceName()
    {
        return MyStringUtils.EndTo(ServerName, "\\");
    }

    public string JustServerName => GetServerName();

    private string GetServerName()
    {
        return MyStringUtils.StartTo(ServerName, "\\");
    }

    public override string ToString()
    {
        return $"{ServerName}\\{DatabaseName} | IntegAuth: {UseIntegratedAuthentication} | User: {UserName}";
    }

    public string ServerAndDatabaseName => $"{ServerName}\\{DatabaseName}";

    #region Equals Implementation
    public bool Equals(BaseSqlConnectionProvider? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return string.Equals(DatabaseName, other.DatabaseName, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(ServerName,   other.ServerName,   StringComparison.OrdinalIgnoreCase) &&
               UseIntegratedAuthentication == other.UseIntegratedAuthentication &&
               string.Equals(UserName, other.UserName, StringComparison.OrdinalIgnoreCase) &&
               string.Equals(Password, other.Password, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((BaseSqlConnectionProvider)obj);
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(DatabaseName, StringComparer.OrdinalIgnoreCase);
        hashCode.Add(ServerName,   StringComparer.OrdinalIgnoreCase);
        hashCode.Add(UseIntegratedAuthentication);
        hashCode.Add(UserName, StringComparer.OrdinalIgnoreCase);
        hashCode.Add(Password, StringComparer.OrdinalIgnoreCase);
        return hashCode.ToHashCode();
    }

    public static bool operator ==(BaseSqlConnectionProvider? left, BaseSqlConnectionProvider? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(BaseSqlConnectionProvider? left, BaseSqlConnectionProvider? right)
    {
        return !Equals(left, right);
    }
    #endregion

    private sealed class DatabaseNameServerNameEqualityComparer : IEqualityComparer<ISqlConnectionProvider>
    {
        public bool Equals(ISqlConnectionProvider x, ISqlConnectionProvider y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.DatabaseName == y.DatabaseName && x.ServerName == y.ServerName;
        }

        public int GetHashCode(ISqlConnectionProvider obj)
        {
            return HashCode.Combine(obj.DatabaseName, obj.ServerName);
        }
    }

    public static IEqualityComparer<ISqlConnectionProvider> DatabaseNameServerNameComparer { get; } = new DatabaseNameServerNameEqualityComparer();

    public virtual SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
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
}