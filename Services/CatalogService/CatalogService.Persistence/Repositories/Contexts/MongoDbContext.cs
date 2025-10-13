using CatalogService.Application.Repositories.Contexts;
using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate;
using CatalogService.Persistence.Models.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CatalogService.Persistence.Repositories.Contexts
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;
        public IClientSessionHandle? Session { get; private set; }

        public MongoDbContext(IMongoClient client, IOptions<MongoDbSettings> options)
        {
            _database = client.GetDatabase(options.Value.DatabaseName);
            _client = client;
        }

        public bool HasActiveTransaction => Session is not null && Session.IsInTransaction;

        public async Task StartSessionAndTransactionAsync(CancellationToken cancellationToken = default)
        {
            Session = await _client.StartSessionAsync(cancellationToken: cancellationToken);
            Session.StartTransaction();
        }
        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (Session?.IsInTransaction == true)
                await Session.CommitTransactionAsync(cancellationToken);
        }
        public async Task AbortTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (Session?.IsInTransaction == true)
                await Session.AbortTransactionAsync(cancellationToken);
        }
        public void Dispose()
        {
            Session?.Dispose();
        }

        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
    }
}
