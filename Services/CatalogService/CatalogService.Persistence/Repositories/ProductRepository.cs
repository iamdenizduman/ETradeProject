using CatalogService.Application.Repositories;
using CatalogService.Application.Repositories.Contexts;
using CatalogService.Domain.Aggregates.ProductAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects;
using MongoDB.Driver;

namespace CatalogService.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoDbContext _context;
        public ProductRepository(IMongoDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetProductsByCategoryId(CategoryId categoryId)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.CategoryId, categoryId);
            return await _context.Products.Find(filter).ToListAsync();
        }
        public async Task UpdateProductAsync(Product product)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
            await _context.Products.ReplaceOneAsync(_context.Session, filter, product, new ReplaceOptions { IsUpsert = false });
        }
    }
}
