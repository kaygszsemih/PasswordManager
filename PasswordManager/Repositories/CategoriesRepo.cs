using PasswordManager.Models;
using PasswordManager.RepositoryManager;

namespace PasswordManager.Repositories
{
    public class CategoriesRepo : GenericRepository<Categories>
    {
        public CategoriesRepo(AppDbContext context) : base(context)
        {
        }
    }
}
