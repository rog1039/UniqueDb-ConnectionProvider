using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniqueDb.ConnectionProvider.DataGeneration;
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
