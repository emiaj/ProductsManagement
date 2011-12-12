using System.Collections.Generic;
using System.Linq;
using FubuCore;
using ProductsManagement.Domain.Entities;

namespace ProductsManagement.Domain
{
    public class InMemoryProductService : IProductService
    {
        private static readonly List<Product> _products;

        private const string LoremIpsum = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod " +
            "tempor incididunt ut labore et dolore magna aliqua. " +
            "Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. " +
            "Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. " +
            "Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        static InMemoryProductService()
        {
            _products = new List<Product>();
            _products.AddRange(Enumerable.Range(1, 23)
                .Select(x => new Product(x)
                {
                    Name = "Product {0}".ToFormat(x),
                    Description = LoremIpsum,
                    Quantity = 20
                }));
        }

        public IQueryable<Product> AllProducts()
        {
            return _products.AsQueryable();
        }

        public void Delete(int id)
        {
            _products.RemoveAll(x => x.Id == id);
        }

        public Product GetById(int id)
        {
            return _products.FirstOrDefault(x => x.Id == id);
        }

        public Product Save(Product product)
        {
            if (product.Id == 0)
            {
                product.Id = _products.Max(x => x.Id) + 1;
                _products.Add(product);
            }
            return product;
        }
    }
}