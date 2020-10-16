using System;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration;
using Xunit;
using Xunit.Abstractions;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration.CSharpGeneration.ServiceLayerGen
{
    public class FormattingTests : UnitTestBaseWithConsoleRedirection
    {
        public static string ClassText = "namespace MyNamespace{public class Hi{public Hi(){}}}";

        [Fact()]
        [Trait("Category", "Instant")]
        public async Task TestFormat()
        {
            var formattedCode = CustomCodeFormattingEngine.Format(ClassText);
            Console.WriteLine(formattedCode);
        }

        public FormattingTests(ITestOutputHelper outputHelperHelper) : base(outputHelperHelper) { }
    }
}