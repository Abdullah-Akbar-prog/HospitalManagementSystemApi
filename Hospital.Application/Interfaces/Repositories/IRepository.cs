namespace Hospital.Application.Interfaces.Repositories
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddAsync(T entity);
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task<bool> DeleteAsync(int id);
        Task<int> CountAsync();
    }
}
