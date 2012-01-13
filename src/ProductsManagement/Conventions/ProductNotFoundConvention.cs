using System;
using System.Linq.Expressions;
using FubuMVC.Core;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Urls;
using ProductsManagement.Handlers.Products;

namespace ProductsManagement.Conventions
{
    public class ProductNotFoundConvention<TOutput> : BasicBehavior where TOutput : class
    {
        private readonly IOutputWriter _outputWriter;
        private readonly IUrlRegistry _urlRegistry;
        private readonly IFubuRequest _fubuRequest;

        public ProductNotFoundConvention(IOutputWriter outputWriter, IUrlRegistry urlRegistry, IFubuRequest fubuRequest)
            : base(PartialBehavior.Executes)
        {
            _outputWriter = outputWriter;
            _urlRegistry = urlRegistry;
            _fubuRequest = fubuRequest;
        }

        protected override DoNext performInvoke()
        {
            if (_fubuRequest.Has<TOutput>() && _fubuRequest.Get<TOutput>()!=null)
            {
                return DoNext.Continue;
            }
            var model = _fubuRequest.Get<NotFoundModel>();
            var url = _urlRegistry.UrlFor(model);
            _outputWriter.RedirectToUrl(url);
            return DoNext.Stop;
        }
    }


    public class ProductNotFoundPolicy<T> : IConfigurationAction
    {
        private readonly Expression<Func<T, object>> _action;

        public ProductNotFoundPolicy(Expression<Func<T, object>> action)
        {
            _action = action;
        }

        public void Configure(BehaviorGraph graph)
        {
            var chain = graph.BehaviorFor(_action);
            if (chain == null || !chain.FirstCall().HasOutput)
            {
                return;
            }
            var node = new Wrapper(typeof (ProductNotFoundConvention<>).MakeGenericType(chain.ActionOutputType()));
            chain.FirstCall().AddAfter(node);
        }
    }
}