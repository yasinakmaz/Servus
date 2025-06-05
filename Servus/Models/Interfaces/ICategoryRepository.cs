namespace Servus.Models.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<ProductCategory>
    {
        Task<IEnumerable<ProductCategory>> GetActiveAndAllCategoryAsync();
        Task<IEnumerable<ProductCategory>> GetActiveSearchCategoryAsync(string searchText);
        Task<IEnumerable<ProductCategory>> GetActiveCategoriesAsync();
        Task<bool> IsCategoryNameExistsAsync(string categoryName, int? excludeId = null);
        Task<IEnumerable<ProductCategory>> GetCategoriesWithProductCountAsync();
    }
}
