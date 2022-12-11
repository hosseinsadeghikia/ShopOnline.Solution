using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ShopOnline.Data
{
    public class ApplicationUser : IdentityUser<int>
    {
        [MaxLength(256)]
        public string FirstName { get; set; }

        [MaxLength(450)]
        public string LastName { get; set; }
    }
}
