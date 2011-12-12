using System.Collections.Generic;
using System.Linq;
using Bottles;
using Bottles.Diagnostics;
using FubuMVC.Core.Registration;

namespace ProductsManagement.Infrastructure
{
    public class ValidationDescriptorProviderFiller : IActivator
    {
        private readonly BehaviorGraph _graph;
        private readonly IValidationDescriptorProvider _provider;

        public ValidationDescriptorProviderFiller(BehaviorGraph graph, IValidationDescriptorProvider provider)
        {
            _graph = graph;
            _provider = provider;
        }

        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            var postActionsFinder = new BehaviorVisitor(_graph.Observer, "Looking for valid POST actions.");
            postActionsFinder.Filters.Add(x => x.Route != null);
            postActionsFinder.Filters.Add(x => x.Route.AllowedHttpMethods.Contains("POST"));
            postActionsFinder.Filters.Add(x => x.FirstCall() != null);
            postActionsFinder.Filters.Add(x => x.FirstCall().HasOutput);
            postActionsFinder.Filters.Add(x => x.FirstCall().HasInput);
            postActionsFinder.Actions += x =>
            {
                var handlerActions = _graph.ActionsForHandler(x.FirstCall().HandlerType);
                var queryAction = handlerActions
                    .Where(h => h.ParentChain().Route != null)
                    .Where(h => h.ParentChain().Route.AllowedHttpMethods.Contains("GET"))
                    .Where(h => h.HasInput).Where(h => h.HasOutput)
                    .Where(h => h.OutputType() == x.InputType())
                    .FirstOrDefault();

                if (queryAction != null)
                {
                    _provider.Register(x.FirstCall(), queryAction.InputType());
                }

            };
            _graph.VisitBehaviors(postActionsFinder);
        }
    }
}