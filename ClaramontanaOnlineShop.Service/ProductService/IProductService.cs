using ClaramontanaOnlineShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClaramontanaOnlineShop.Service.ProductService
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(Guid id);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);

    }
}