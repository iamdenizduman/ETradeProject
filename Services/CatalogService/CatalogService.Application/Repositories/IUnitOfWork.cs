using CatalogService.Domain.Abstracts;
using MongoDB.Driver;

namespace CatalogService.Application.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        bool HasActiveTransaction { get; }
        ICategoryRepository CategoryRepository { get; }
        IProductRepository ProductRepository { get; }
        Task StartSessionAndTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task AbortTransactionAsync(CancellationToken cancellationToken = default);
        IEnumerable<BaseAggregateRoot> GetTrackedEntitiesWithEvents();
        void TrackEntity(BaseAggregateRoot entity);
    }
}
