namespace PasswordManager.RepositoryManager
{
    public interface IGenericRepository<T> where T : class
    {
        Task CreateAsync(T entity);

        Task<T> GetByIdAsync(int id);

        IQueryable<T> GetAllAsync();

        void Update(T entity);

        void Delete(T entity);
    }
}
