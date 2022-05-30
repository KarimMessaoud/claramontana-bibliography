using ClaramontanaOnlineShop.Data;
using ClaramontanaOnlineShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClaramontanaOnlineShop.Service.ProductService
{
    public class ProductService : IProductService
    {
        private readonly ProductContext _dbContext;
        public ProductService(ProductContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateProductAsync(Product product)
        {
            _dbContext.Add(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            _dbContext.Remove(product);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await _dbContext.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {

            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            var item = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == product.Id);

            item.Name = product.Name;
            item.Description = product.Description;
            item.Price = product.Price;
            item.Image = product.Image;
            item.Quantity = product.Quantity;
            item.IsAvailable = product.IsAvailable;

            await _dbContext.SaveChangesAsync();
        }
    }           
}
