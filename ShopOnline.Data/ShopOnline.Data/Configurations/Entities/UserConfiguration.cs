using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ShopOnline.Data.Configurations.Entities
{
    public class UserConfiguration : IEntityTypeConfiguration<ApplicationUser>
    {
        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasData(
                new ApplicationUser
                {
                    Id = 1,
                    UserName = "Bob",
                    FirstName = "Bob",
                    LastName = "Uncle"
                    
                },
                new ApplicationUser
                {
                    Id = 2,
                    UserName = "Sarah",
                    FirstName = "Sarah",
                    LastName = "William"
                }
            );
        }
    }
}
