using Xunit;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration
{
    public class SqlTableFactoryTests
    {
        [Fact()]
        [Trait("Category", "Integration")]
        public void CreateFromQueryTests()
        {
            var query = "SELECT * FROM sys.types";
        }
    }
}
