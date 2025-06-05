namespace Servus.Models.Interfaces
{
    public interface IProductHandler
    {
        Task<ProductDto> CreateProductAsync(CreateProductDto createDto);
        Task<ProductDto> UpdateProductAsync(int id, UpdateProductDto updateDto);
        Task<bool> DeleteProductAsync(int id);
        Task<ProductDto?> GetProductAsync(int id);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    }
}
