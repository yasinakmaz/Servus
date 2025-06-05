namespace Servus.Models.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Categories>> GetActiveAndAllCategoryAsync();
        Task<IEnumerable<Categories>> GetActiveSearchCategoryAsync(string searchText);
    }
}
