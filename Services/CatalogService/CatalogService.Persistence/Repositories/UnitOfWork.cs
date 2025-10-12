using CatalogService.Application.Repositories;
using MongoDB.Driver;

namespace CatalogService.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoClient _client;
        private IClientSessionHandle? _session;
        public bool HasActiveTransaction => _session is not null && _session.IsInTransaction;

        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get;  }

        public UnitOfWork(IMongoClient client, ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _client = client; 
            CategoryRepository = categoryRepository;
            ProductRepository = productRepository;
        }

        public async Task StartSessionAndTransactionAsync(CancellationToken cancellationToken = default)
        {
            _session = await _client.StartSessionAsync(cancellationToken: cancellationToken);
            _session.StartTransaction();
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_session?.IsInTransaction == true)
                await _session.CommitTransactionAsync(cancellationToken);
        }

        public async Task AbortTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_session?.IsInTransaction == true)
                await _session.AbortTransactionAsync(cancellationToken);
        }

        public void Dispose()
        {
            _session?.Dispose();
        }
    }
}
