using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using UniqueDb.ConnectionProvider.DataGeneration;
using Xunit;
using Xunit.Abstractions;

namespace UniqueDb.ConnectionProvider.Tests
{
    public class GenerateAttributeCheckingCode : UnitTestBaseWithConsoleRedirection
    {
        [Fact()]
        [Trait("Category", "Instant")]
        public void GenerateCode()
        {
            var assembly      = typeof(NotMappedAttribute).Assembly;
            var assemblyTypes = assembly
                .GetExportedTypes()
                .Where(z => z.Name.EndsWith("Attribute"))
                .ToList();
            assemblyTypes.PrintStringTable();

        }

        public GenerateAttributeCheckingCode(ITestOutputHelper outputHelperHelper) : base(outputHelperHelper) { }
    }
}