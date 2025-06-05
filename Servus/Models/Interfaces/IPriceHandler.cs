namespace Servus.Models.Interfaces
{
    public interface IPriceHandler
    {
        Task<PriceDto?> CreatePriceAsync(CreatePriceDto createDto);
        Task<PriceDto?> UpdatePriceAsync(int id, UpdatePriceDto updateDto);
        Task<bool> DeletePriceAsync(int id);
        Task<PriceDto?> GetPriceAsync(int id);
        Task<IEnumerable<PriceDto>> GetAllPricesAsync();
        Task<IEnumerable<PriceListDto>> GetPriceListAsync();
        Task<IEnumerable<PriceDto>> GetActivePricesAsync();
        Task<IEnumerable<PriceDto>> GetFirstChoicePricesAsync();
        Task<IEnumerable<PriceDto>> GetSecondChoicePricesAsync();
        Task<bool> SetDefaultPriceAsync(int priceId);
    }
}
