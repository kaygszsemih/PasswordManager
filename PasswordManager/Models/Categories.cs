namespace PasswordManager.Models
{
    public class Categories : BaseEntity
    {
        public string CategoryName { get; set; }

        public string UserID { get; set; }

        public AppUser AppUser { get; set; }

        public List<MyPasswords> MyPasswords { get; set; }
    }
}
