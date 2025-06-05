namespace Servus.Models.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIDAsync(int IND);
        Task<T?> FindAsync(Expression<Func<T, bool>> predicate);
        Task<T?> AddAsync(T entity);
        Task<T?> UpdateAsync(T entity);
        Task<bool> DeleteAsync(int IND);
        Task<bool> ExistsAsync(int IND);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> predicate);
        Task<T?> FindFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
    }
}
