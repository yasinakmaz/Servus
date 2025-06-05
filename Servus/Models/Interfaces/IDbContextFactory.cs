namespace Servus.Models.Interfaces
{
    public interface IDbContextFactory
    {
        ProductDbContext CreateContext(DatabaseProvider provider);
    }
}
