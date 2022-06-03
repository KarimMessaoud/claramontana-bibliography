using ClaramontanaOnlineShop.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;


namespace ClaramontanaOnlineShop.Data.EntityTypesConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        //Configuration based on the following article: https://joshthecoder.com/2020/04/28/max-data-types-in-aspnet-core-identity-schema.html
        //and: https://github.com/dotnet/aspnetcore/issues/5823

        public void Configure(EntityTypeBuilder<User> builder)
            {
                builder.Property(x => x.PhoneNumber).HasMaxLength(50)
                                                    .IsUnicode(false)
                                                    .IsFixedLength(false);

                builder.Property(x => x.PasswordHash).HasMaxLength(84)
                                                     .IsUnicode(false)
                                                     .IsFixedLength(true);

                builder.Property(x => x.SecurityStamp).HasMaxLength(36)
                                                      .IsUnicode(false)
                                                      .IsFixedLength(false)
                                                      .IsRequired(true);

                builder.Property(x => x.ConcurrencyStamp).HasMaxLength(36)
                                                         .IsUnicode(false)
                                                         .IsFixedLength(true)
                                                         .IsRequired(true);
            }
    }
}
