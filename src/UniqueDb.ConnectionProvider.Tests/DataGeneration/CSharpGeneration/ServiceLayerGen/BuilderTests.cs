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
            var domainProj    = builder.CreateProject("F:\\wgtest\\CodeGenTestSol1\\CodeGenTestSol1.Project1", "CodeGenTestSol1.Project1", "Features");
            var webApiProj    = builder.CreateProject("F:\\wgtest\\CodeGenTestSol1\\CodeGenTestSol1.WebApiProj1", "CodeGenTestSol1.WebApiProj1", "");
            var coreFeature = builder.CreateFeature("MainFeature.SubFeature1");
            var webApiFeature = builder.CreateFeature("Controllers.SubFeature1");

            var origInterface = builder.ReadType(typeof(IMyTestService));
            origInterface.FullName = "CodeGenTestSol1.Project1.Features.MainFeature.SubFeature1.IMyTestService";

            var impl = builder.CreateClass(domainProj, coreFeature);
            impl.NamedBy(origInterface, suffix: "Impl");
            impl.Implements(origInterface);

            var apiController = builder.CreateApiController(webApiProj, webApiFeature);
            apiController.NamedBy(origInterface, suffix: "ApiController");
            apiController.DelegatesTo(impl);
            apiController.BaseTypeName = "ApiController";

            var apiDef = builder.CreateApiDef(domainProj, coreFeature);
            apiDef.NamedBy(origInterface, suffix: "ApiDef");
            apiDef.TalksTo = apiController;

            var stub = builder.CreateInterface(domainProj, coreFeature);
            stub.HasBaseInterface(origInterface);
            stub.BaseTypeName = origInterface.Name;
            stub.NamedBy(origInterface, suffix:"Stub");

            var fromApi = builder.CreateClass(domainProj, coreFeature);
            fromApi.NamedBy(stub, suffix:"FromApi");
            fromApi.DelegatesTo(apiDef);
            fromApi.Implements(stub);

            var fromDb = builder.CreateClass(domainProj, coreFeature);
            fromDb.NamedBy(stub, suffix: "FromDb");
            fromDb.DelegatesTo(impl);
            fromDb.Implements(stub);


            var realizer = new ConfigRealizer();
            var results  = realizer.RealizeConfig(builder);

            results.PrintStringTable();
            foreach (var fileResult in results)
            {
                fileResult.SaveToDisk();
            }
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