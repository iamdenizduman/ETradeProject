using MongoDB.Driver;

namespace CatalogService.Application.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        bool HasActiveTransaction { get; }
        Task StartSessionAndTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task AbortTransactionAsync(CancellationToken cancellationToken = default);
    }
}
