namespace Servus.Models.Interfaces
{
    public interface IProductSecondChoiceRepository : IGenericRepository<ProductSecondChoice>
    {
        Task<IEnumerable<ProductSecondChoice>> GetByProductIdAsync(int productId);
        Task<IEnumerable<ProductSecondChoice>> GetActiveByProductIdAsync(int productId);
        Task<bool> DeleteByProductIdAsync(int productId);
    }
}
