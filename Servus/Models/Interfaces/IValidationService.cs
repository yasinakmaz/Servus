namespace Servus.Models.Interfaces
{
    public interface IValidationService
    {
        Task<bool> ValidateProductCodeAsync(string productCode, int? excludeId = null);
        Task<bool> ValidateProductNameAsync(string productName, int? excludeId = null);
        Task<bool> ValidateCategoryNameAsync(string categoryName, int? excludeId = null);
        Task<bool> ValidatePriceNameAsync(string priceName, int? excludeId = null);
        Task<bool> ValidateCreateProductAsync(CreateProductDto dto);
        Task<bool> ValidateUpdateProductAsync(int productId, UpdateProductDto dto);
    }
}
