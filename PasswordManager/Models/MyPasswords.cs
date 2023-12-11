using Microsoft.AspNetCore.Identity;

namespace PasswordManager.Models
{
    public class MyPasswords : BaseEntity
    {
        public string Description { get; set; }

        public string URL { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int CategoryID { get; set; }

        public Categories Categories { get; set; }

        public string UserID { get; set; }

        public AppUser AppUser { get; set; }
    }
}
