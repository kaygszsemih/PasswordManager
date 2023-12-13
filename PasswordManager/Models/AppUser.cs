using Microsoft.AspNetCore.Identity;

namespace PasswordManager.Models
{
    public class AppUser : IdentityUser
    {
        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public List<Categories> Categories { get; set; }

        public List<MyPasswords> MyPasswords { get; set; }

    }
}
