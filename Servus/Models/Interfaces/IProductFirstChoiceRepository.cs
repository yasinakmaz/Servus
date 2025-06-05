namespace Servus.Models.Interfaces
{
    public interface IProductFirstChoiceRepository : IGenericRepository<ProductFirstChoice>
    {
        Task<IEnumerable<ProductFirstChoice>> GetByProductIdAsync(int productId);
        Task<IEnumerable<ProductFirstChoice>> GetActiveByProductIdAsync(int productId);
        Task<bool> DeleteByProductIdAsync(int productId);
    }
}
