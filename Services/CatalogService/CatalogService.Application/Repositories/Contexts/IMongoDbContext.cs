using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate;
using MongoDB.Driver;

namespace CatalogService.Application.Repositories.Contexts
{
    public interface IMongoDbContext
    {
        IClientSessionHandle? Session { get; }
        void Dispose();
        bool HasActiveTransaction { get; }
        Task StartSessionAndTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task AbortTransactionAsync(CancellationToken cancellationToken = default);
        IMongoCollection<Category> Categories { get; }
        IMongoCollection<Product> Products { get; }
    }
}
