using Microsoft.AspNetCore.Identity;

namespace PasswordManager.Models
{
    public class AppUser : IdentityUser
    {
        public List<Categories> Categories { get; set; }

        public List<MyPasswords> MyPasswords { get; set; }

    }
}
