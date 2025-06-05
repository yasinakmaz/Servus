namespace Servus.Models.Interfaces
{
    public interface IPriceRepository : IGenericRepository<Price>
    {
        Task<IEnumerable<Price>> GetActivePricesAsync();
        Task<IEnumerable<Price>> GetFirstChoicePricesAsync();
        Task<IEnumerable<Price>> GetSecondChoicePricesAsync();
        Task<Price?> GetDefaultPriceAsync();
        Task<bool> IsPriceNameExistsAsync(string priceName, int? excludeId = null);
        Task<bool> SetDefaultPriceAsync(int priceId);
    }
}
