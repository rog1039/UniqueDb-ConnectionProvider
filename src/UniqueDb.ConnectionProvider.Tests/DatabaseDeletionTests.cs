using System;
using System.Data.SqlClient;
using Dapper;
using Xbehave;

namespace UniqueDb.ConnectionProvider.Tests
{
    public class DatabaseDeletionTests
    {
        [Scenario]
        public void ShouldDispose()
        {
            var options = new UniqueDbConnectionProviderOptions("ws2012sqlexp1\\sqlexpress", "autodisposedatabase");
            var connectionProvider = new UniqueDbConnectionProvider(options);

            "After creating a database"
                ._(() => connectionProvider.CreateDatabase());

            "Disposing of the disposable provided by the ToDisposable extension method should delete the database"
                ._(() =>
                {
                    using (var lifecycle = connectionProvider.ToDisposable())
                    {
                        var result = connectionProvider.ExecuteScalar<int>("SELECT 1 ");
                    }
                });
        }
    }
}