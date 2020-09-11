using System;
using System.Threading.Tasks;

namespace UniqueDb.ConnectionProvider.DataGeneration.CSharpGeneration.ServiceLayerGen
{
    public class ClassDescription
    {
        public string ClassName { get; set; }
        public string Namespace { get; set; }
    }
    
    /*
     * How would we write this?
     *
     * var config = new FileBuilderConfig();
     * config.SetBase(typeof(X))
     *     .WithName("MyService");;
     * config.AddInterface("MyNamespace", "some/on/disk/path")
     *     .WithSuffix("Stub");
     * config.AddInterface("MyNamespace", "some/on/disk/path")
     *     .WithSuffix("FromApi");
     * config.AddInterface("MyNamespace", "some/on/disk/path")
     *     .WithSuffix("FromDb");
     *
     *
     * var domainProj = config.CreateProject("LogisticsProjRoot", "Woeber.Logistics", "");
     * var webApiProj = config.CreateProject("ProjectRootDir", "ProjectName");
     * var dataProj = config.CreateProject("DataProjRoot", "Woeber.Logistics.Data");
     * var appProject = config.CreateProject("LogisticsWpfProjRoot", "Woeber.Logistics.App", "Features");
     * var feature = config.CreateFeature("DbSync.Management");
     *
     * config
     *     .SetDefaults(ApiController)
     *     .Attribute("[Route(\"api[controller]\")"])
     *     .Attribute("[Authorize(Policy = WebApiConstants.ControllerActionVerbPolicyName)]")
     *     .SetSuffix("ApiController)
     *     .SetBaseClass("MyApiController");
     * 
     * var origInterface = config.ReadInterface(domainProj, feature, typeof(IDbConnectionService));
     *     -- Stick to just methods at first to KISS.
     * 
     * var impl = config.CreateClass(dataProj, feature, suffix: "Impl");
     * impl.Implements(origInterface).AsThrowNowImplemented();
     *
     * var apiController = config.CreateApiController(webApiProj, feature);
     * apiController.DelegatesTo(impl).All();
     * 
     * var apiDef = config.CreateApiDef("Namespace", suffix: "ApiDef");
     * apiDef.TalksTo(apiController);
     *
     * var stub = config.CreateInterface("Namespace", suffix: "Stub");
     * stub.Implements(object);
     * 
     * var fromApi = config.CreateClass("Namespace", suffix: "FromApi");
     * fromApi.InheritsFrom(stub);
     * fromApi.DelegatesTo(apiDef);
     *
     * var fromDb = config.CreateClass("Namespace", suffix: "FromDb");
     * fromDb.Implements(stub);
     * fromDb.DelegatesTo(impl);
     *
     * var results = config.Apply
     * 
     */

    public interface IMyService
    {
        Task<DateTime> GetDate();
    }

    public interface IMyServiceStub : IMyService { }

    public class MyServiceUsingApi : IMyServiceStub
    {
        private readonly IMyServiceApiDef _apiDef;
        
        public MyServiceUsingApi(IMyServiceApiDef apiDef)
        {
            _apiDef = apiDef;
        }

        public Task<DateTime> GetDate()
        {
            return _apiDef.GetDate();
        }
    }

    public interface IMyServiceApiDef
    {
        Task<DateTime> GetDate();
    }

    public class MyServiceApiController
    {
        private readonly MyServiceImpl _myServiceImpl;

        public Task<DateTime> GetDate()
        {
            return _myServiceImpl.GetDate();
        }
    }

    public class MyServiceUsingDb : IMyServiceStub
    {
        private readonly MyServiceImpl _myServiceImpl;
        
        public MyServiceUsingDb(MyServiceImpl myServiceImpl)
        {
            _myServiceImpl = myServiceImpl;
        }

        public Task<DateTime> GetDate()
        {
            return _myServiceImpl.GetDate();
        }
    }

    public class MyServiceImpl : IMyService
    {
        public async Task<DateTime> GetDate()
        {
            return DateTime.UtcNow;
        }
    }
}