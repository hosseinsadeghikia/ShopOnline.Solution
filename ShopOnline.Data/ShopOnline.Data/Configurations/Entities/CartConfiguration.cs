using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShopOnline.Data.Configurations.Entities
{
    public class CartConfiguration : IEntityTypeConfiguration<Cart>
    {
        public void Configure(EntityTypeBuilder<Cart> builder)
        {
            builder.HasData(
                new Cart
                {
                    Id = 1,
                    UserId = 1
                },
                new Cart
                {
                    Id = 2,
                    UserId = 2
                }
            );
        }
    }
}
