using PasswordManager.Models;
using PasswordManager.RepositoryManager;

namespace PasswordManager.Repositories
{
    public class MyPasswordsRepo : GenericRepository<MyPasswords>
    {
        public MyPasswordsRepo(AppDbContext context) : base(context)
        {
        }
    }
}
