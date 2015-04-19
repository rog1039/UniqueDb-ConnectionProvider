using System;
using System.Data.SqlClient;

namespace UniqueDb.ConnectionProvider
{
    /// <summary>
    /// A class that provides a connection string to a brand-new database.
    /// </summary>
    public class UniqueDbConnectionProvider : BaseSqlConnectionProvider
    {
        private readonly UniqueDbConnectionProviderOptions _options;

        /// <summary>
        /// The string format used to created the database name.
        /// </summary>
        public string DatabaseNameFormatString { get; set; }

        public UniqueDbConnectionProvider(UniqueDbConnectionProviderOptions options)
        {
            _options = options;
            ServerName = _options.SqlServerName;
            DatabaseNameFormatString = "{0}-({1})-{2:n}";
            DatabaseName = GenerateDbName();
        }

        private string GenerateDbName()
        {
            var prefix = _options.DatabaseNamePrefix;
            var timestamp = _options.IncludeTimeStamp ? DateTime.Now.ToString(_options.TimeStampFormat) : string.Empty;
            var guid = Guid.NewGuid();

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
            connString.DataSource = _options.SqlServerName;
            connString.InitialCatalog = DatabaseName;
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
    }
}
