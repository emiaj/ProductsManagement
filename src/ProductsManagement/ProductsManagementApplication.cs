using FubuMVC.Core;
using FubuMVC.StructureMap;
using FubuValidation.StructureMap;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace ProductsManagement
{
    public class ProductsManagementApplication : IApplicationSource
    {
        public FubuApplication BuildApplication()
        {
            var container = new Container();
            container
                .Configure(x =>
                {
                    var registry = new Registry();
                    registry.FubuValidation();
                    x.AddRegistry(registry);
                });
            return FubuApplication
                .For<CoreRegistry>()
                .StructureMap(container);
        }
    }
}