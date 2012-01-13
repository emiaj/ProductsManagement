using System.Collections.Generic;
using Bottles;
using Bottles.Diagnostics;
using FubuValidation;
using ProductsManagement.Handlers.Products;

namespace ProductsManagement.Infrastructure
{
    public class LocalizationActivator : IActivator
    {
        private readonly ILocaleCacheFactory _localeCacheFactory;

        public LocalizationActivator(ILocaleCacheFactory localeCacheFactory)
        {
            _localeCacheFactory = localeCacheFactory;
        }

        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            var cache = _localeCacheFactory.GetDefault();
            cache.Append(ValidationKeys.REQUIRED, "Campo requerido");
            cache.Append<EditProductModel>(p => p.Description, "Descripción");
            //cache.Append<AddProductModel>(p => p.Description, "Descripción");
        }
    }
}