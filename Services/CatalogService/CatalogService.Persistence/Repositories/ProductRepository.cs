using CatalogService.Application.Repositories;
using CatalogService.Domain.Aggregates.ProductAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects;
using CatalogService.Persistence.Repositories.Contexts;
using MongoDB.Driver;

namespace CatalogService.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly MongoDbContext _context;
        public ProductRepository(MongoDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(CategoryId categoryId)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.CategoryId, categoryId);
            return await _context.Products.Find(filter).ToListAsync();
        }
        public async Task UpdateProductAsync(Product product, IClientSessionHandle session)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
            await _context.Products.ReplaceOneAsync(session, filter, product, new ReplaceOptions { IsUpsert = false });
        }
    }
}
