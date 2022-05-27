using ClaramontanaOnlineShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClaramontanaOnlineShop.Data
{
    public class ProductContext : DbContext
    {
        public ProductContext(DbContextOptions<ProductContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
