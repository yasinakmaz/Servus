namespace Servus.Models.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetActiveAndAllProductAsync();
        Task<IEnumerable<Product>> GetActiveByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetActiveSearchProductAsync(string searchText);
        Task<IEnumerable<Product>> GetProductsWithDetailsAsync();
        Task<Product?> GetProductWithDetailsAsync(int productId);
        Task<bool> IsProductCodeExistsAsync(string productCode, int? excludeId = null);
        Task<bool> IsProductNameExistsAsync(string productName, int? excludeId = null);
    }
}
