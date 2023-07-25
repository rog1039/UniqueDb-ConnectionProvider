using UniqueDb.ConnectionProvider.SqlScripting;

namespace UniqueDb.ConnectionProvider;

public class SqlConnectionDbDeletingDisposable : IDisposable
{
    private readonly ISqlConnectionProvider _dbConnectionProvider;

    public SqlConnectionDbDeletingDisposable(ISqlConnectionProvider dbConnectionProvider)
    {
        _dbConnectionProvider = dbConnectionProvider;
    }

    public void Dispose()
    {
        DatabaseDeleter.DeleteDatabase(_dbConnectionProvider);
    }
}