namespace Servus.Models.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IPriceRepository Prices { get; }
        IProductFirstChoiceRepository ProductFirstChoices { get; }
        IProductSecondChoiceRepository ProductSecondChoices { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
