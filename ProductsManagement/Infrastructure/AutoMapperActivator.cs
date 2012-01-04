using System.Collections.Generic;
using AutoMapper;
using Bottles;
using Bottles.Diagnostics;
using ProductsManagement.Domain.Entities;
using ProductsManagement.Handlers.Products;

namespace ProductsManagement.Infrastructure
{
    public class AutoMapperActivator : IActivator
    {
        private readonly IConfiguration _configuration;

        public AutoMapperActivator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Activate(IEnumerable<IPackageInfo> packages, IPackageLog log)
        {
            _configuration.CreateMap<Product, EditProductModel>();
            _configuration.CreateMap<EditProductCommandModel, Product>();

            _configuration.CreateMap<Product, AddProductModel>();
            _configuration.CreateMap<AddProductModel, Product>();
        }
    }
}