using Xbehave;

namespace UniqueDb.ConnectionProvider.Tests
{
    public class SqlConnectionProviderExtensionMethodsTests
    {
        [Scenario]
        public void TestExecute()
        {
            var options = new UniqueDbConnectionProviderOptions("ws2012sqlexp1\\sqlexpress", "autodisposedatabase");
            var connectionProvider = new UniqueDbConnectionProvider(options);
            using (connectionProvider.ToDisposable())
            {
                connectionProvider.CreateDatabase();
                connectionProvider.ExecuteNonDapper("Use [" + connectionProvider.DatabaseName + "]");
            }
        }

        [Scenario]
        public void TestExecuteScalar()
        {
            var options = new UniqueDbConnectionProviderOptions("ws2012sqlexp1\\sqlexpress", "autodisposedatabase");
            var connectionProvider = new UniqueDbConnectionProvider(options);
            using (connectionProvider.ToDisposable())
            {
                connectionProvider.CreateDatabase();
                connectionProvider.ExecuteScalar<int>("Select 1");
            }
        }
    }
}