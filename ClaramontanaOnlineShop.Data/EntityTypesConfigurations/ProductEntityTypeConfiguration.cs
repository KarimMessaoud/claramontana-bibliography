using ClaramontanaOnlineShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;


namespace ClaramontanaOnlineShop.Data.EntityTypesConfigurations
{
    public class ProductEntityTypeConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(100)
                                         .IsRequired(true);

            builder.Property(x => x.Description).HasMaxLength(500);

            builder.Property(x => x.Price).HasColumnType("decimal(10,2)")
                                          .IsRequired(true);

            builder.Property(x => x.Image).HasMaxLength(500)
                                          .IsRequired(true);

            builder.Property(x => x.Quantity).IsRequired(true);

            builder.Property(x => x.IsAvailable).IsRequired(true);
        }
    }
}
