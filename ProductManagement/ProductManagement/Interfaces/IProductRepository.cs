using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductManagement.Interfaces
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
        bool InsertProduct(Product product);
        bool UpdateProduct(Product product);
        bool IsExistedProduct(int id);
        bool DeleteProductById(int id);
    }
}
