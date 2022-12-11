using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShopOnline.Data.Configurations.Entities
{
    public class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.HasData(
                new ProductCategory
                {
                    Id = 1,
                    Name = "Beauty",
                    IconCss = "fa fa-spa"
                },
                new ProductCategory
                {
                    Id = 2,
                    Name = "Furniture",
                    IconCss = "fa fa-couch"
                },
                new ProductCategory
                {
                    Id = 3,
                    Name = "Electronics",
                    IconCss = "fa fa-headphones"
                },
                new ProductCategory
                {
                    Id = 4,
                    Name = "Shoes",
                    IconCss = "fa fa-shoe-prints"
                }
            );
        }
    }
}
