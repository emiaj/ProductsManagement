using System.Linq;
using ProductsManagement.Domain.Entities;

namespace ProductsManagement.Domain
{
    public interface IProductService
    {
        IQueryable<Product> AllProducts();
        void Delete(int id);
        Product GetById(int id);
        Product Save(Product product);
    }
}