using CatalogService.Application.Repositories;
using CatalogService.Application.Repositories.Contexts;
using CatalogService.Domain.Abstracts;

namespace CatalogService.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IMongoDbContext _context;
        private readonly List<BaseAggregateRoot> _trackedEntities = new();

        public ICategoryRepository CategoryRepository { get; }
        public IProductRepository ProductRepository { get; }

        public bool HasActiveTransaction => _context.HasActiveTransaction;  

        public UnitOfWork(IMongoDbContext context, ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _context = context;
            CategoryRepository = categoryRepository;
            ProductRepository = productRepository;
        }

        public async Task StartSessionAndTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.StartSessionAndTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.CommitTransactionAsync(cancellationToken);
        }

        public async Task AbortTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.AbortTransactionAsync(cancellationToken);
        }

        public void TrackEntity(BaseAggregateRoot entity)
        {
            if (!_trackedEntities.Contains(entity))
                _trackedEntities.Add(entity);
        }

        public IEnumerable<BaseAggregateRoot> GetTrackedEntitiesWithEvents()
        {
            return _trackedEntities.Where(e => e.DomainEvents != null && e.DomainEvents.Any());
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
