using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniqueDb.ConnectionProvider.DataGeneration;
using UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration.ServiceLayerGen;
using Xunit;
using Xunit.Abstractions;

namespace UniqueDb.ConnectionProvider.Tests.DataGeneration.CSharpGeneration.ServiceLayerGen
{
    public class BuilderTests : UnitTestBaseWithConsoleRedirection
    {
        [Fact()]
        [Trait("Category", "Instant")]
        public void name()
        {
            var builder = new ConfigBuilder();
            var proj    = builder.CreateProject("F:\\wg\\CronNET\\CronNET", "CronNET", "Features");
            var feature = builder.CreateFeature("MainFeature.SubFeature");

            var origInterface = builder.ReadInterface(typeof(IMyTestService));

            var impl = builder.CreateClass(proj, feature);
            impl.NamedBy(origInterface, suffix: "Impl");
            impl.Implements(origInterface);

            var apiController = builder.CreateApiController(proj, feature);
            apiController.NamedBy(origInterface, suffix: "ApiController");
            apiController.DelegatesTo(impl);
            apiController.BaseTypeName = "ApiController";

            var realizer = new ConfigRealizer();
            var results  = realizer.RealizeConfig(builder);

            results.PrintStringTable();
        }

        [Fact()]
        [Trait("Category", "Instant")]
        public async Task TestGenericNames()
        {
            var returnTypeName = typeof(IMyTestService).GetMethods()
                .Select(z => z.ReturnType.GetTypeName())
                .ToList();

            returnTypeName.Select(z => new {Name = z}).PrintStringTable();
        }

        public BuilderTests(ITestOutputHelper outputHelperHelper) : base(outputHelperHelper)
        {
        }
    }

    public interface IMyTestService
    {
        Task<DateTime> GetCurrentDate();
        Task<List<DateTime>> GetCurrentDates();
        Task SetCurrentDate(DateTime date);
    }
}