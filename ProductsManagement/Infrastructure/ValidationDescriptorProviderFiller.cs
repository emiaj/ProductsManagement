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
                var postAction = x.FirstCall();
                var handlerActions = _graph.ActionsForHandler(postAction.HandlerType);
                var getAction = handlerActions
                    .Where(h => h.ParentChain().Route != null)
                    .Where(h => h.ParentChain().Route.AllowedHttpMethods.Contains("GET"))
                    .Where(h => h.HasInput).Where(h => h.HasOutput)
                    .FirstOrDefault(h => x.InputType().IsAssignableFrom(h.OutputType()));

                if (getAction == null)
                {
                    return;
                }
                log.Trace("Linking validation descriptor for {0} against {1}.", postAction, getAction);
                _provider.Register(postAction, getAction.InputType());
            };
            _graph.VisitBehaviors(postActionsFinder);
        }
    }
}