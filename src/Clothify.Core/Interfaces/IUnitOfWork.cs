namespace Clothify.Core.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IProductRepository Products { get; }
    IOrderRepository Orders { get; }
    ICartRepository Cart { get; }
    IRepository<T> Repository<T>() where T : class;
    Task<int> SaveChangesAsync();
}
