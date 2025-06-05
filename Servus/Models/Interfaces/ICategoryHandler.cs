namespace Servus.Models.Interfaces
{
    public interface ICategoryHandler
    {
        Task<ProductCategoryDto?> CreateCategoryAsync(CreateProductCategoryDto createDto);
        Task<ProductCategoryDto?> UpdateCategoryAsync(int id, UpdateProductCategoryDto updateDto);
        Task<bool> DeleteCategoryAsync(int id);
        Task<ProductCategoryDto?> GetCategoryAsync(int id);
        Task<IEnumerable<ProductCategoryDto>> GetAllCategoriesAsync();
        Task<IEnumerable<CategoryListDto>> GetCategoryListAsync();
        Task<IEnumerable<ProductCategoryDto>> GetActiveCategoriesAsync();
        Task<IEnumerable<ProductCategoryDto>> SearchCategoriesAsync(string searchText);
    }
}
