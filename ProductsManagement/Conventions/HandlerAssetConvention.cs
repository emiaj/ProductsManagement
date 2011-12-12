using System.Linq;
using FubuCore;
using FubuMVC.Core;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Http;

namespace ProductsManagement.Conventions
{
    public class HandlerAssetConvention : BasicBehavior
    {
        private readonly ICurrentChain _currentChain;
        private readonly IAssetRequirements _assetRequirements;

        public HandlerAssetConvention(ICurrentChain currentChain, IAssetRequirements assetRequirements)
            : base(PartialBehavior.Ignored)
        {
            _currentChain = currentChain;
            _assetRequirements = assetRequirements;
        }

        protected override DoNext performInvoke()
        {
            var call = _currentChain.Current.FirstCall();
            var handlerNs = call.HandlerType.Namespace.Split('.').Last().ToLower();
            var handlerName = call.HandlerType.Name.Replace("Handler", "").ToLower();
            var groupName = "{0}.{1}".ToFormat(handlerNs, handlerName);
            var scriptName = "{0}.scripts".ToFormat(groupName);
            var styleName = "{0}.styles".ToFormat(groupName);
            _assetRequirements.Require(scriptName, styleName);
            return DoNext.Continue;
        }
    }
}