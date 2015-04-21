using System;

namespace UniqueDb.ConnectionProvider
{
    public class SqlConnectionDbDisposable : IDisposable
    {
        private readonly ISqlConnectionProvider _dbConnectionProvider;

        public SqlConnectionDbDisposable(ISqlConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public void Dispose()
        {
            DatabaseDeleter.DeleteDatabase(_dbConnectionProvider);
        }
    }
}
