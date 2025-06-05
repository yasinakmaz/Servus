namespace Servus.Models.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Products>> GetActiveAndAllProduct();
        Task<IEnumerable<Products>> GetActiveAndCategoryAsync(int CategoryIND);
        Task<IEnumerable<Products>> GetActiveSearchProduct(string searchText);
    }
}
