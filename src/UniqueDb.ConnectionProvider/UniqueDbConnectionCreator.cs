using System;
using System.Data.SqlClient;

namespace UniqueDb.ConnectionProvider
{
    /// <summary>
    /// A class that provides a connection string to a brand-new database.
    /// </summary>
    public class UniqueDbConnectionCreator
    {
        private readonly UniqueDbConnectionProviderOptions _options;

        /// <summary>
        /// The database name.
        /// </summary>
        public string DbName { get; private set; }

        public UniqueDbConnectionCreator(UniqueDbConnectionProviderOptions options)
        {
            _options = options;
            DbName = GenerateDbName();
        }

        private string GenerateDbName()
        {
            var prefix = _options.DatabaseNamePrefix;
            var timestamp = _options.IncludeTimeStamp ? DateTime.Now.ToString(_options.TimeStampFormat) : string.Empty;
            var guid = Guid.NewGuid();

            var generatedName = string.Format("{0}-({1})-{2:n}",
                prefix,
                timestamp,
                guid);

            return generatedName;
        }

        /// <summary>
        /// Returns the SqlConnectionStringBuilder for this unique Db.
        /// </summary>
        /// <returns></returns>
        public SqlConnectionStringBuilder GetSqlConnectionStringBuilder()
        {
            var connString = new SqlConnectionStringBuilder();
            connString.DataSource = _options.SqlServerName;
            connString.InitialCatalog = DbName;
            if (_options.UseIntegratedSecurity)
            {
                connString.IntegratedSecurity = true;
            }
            else
            {
                connString.UserID = _options.UserName;
                connString.Password = _options.Password.ToString();
            }
            return connString;
        }

        /// <summary>
        /// Returns a new SqlConnection pointed to this unique Db.
        /// </summary>
        /// <returns></returns>
        public SqlConnection GetSqlConnection()
        {
            var connectionString = GetSqlConnectionString();
            return new SqlConnection(connectionString);
        }

        /// <summary>
        /// Returns a new SqlConnection string pointed to this unique Db.
        /// </summary>
        /// <returns></returns>
        public string GetSqlConnectionString()
        {
            return GetSqlConnectionStringBuilder().ConnectionString;
        }
    }
}
