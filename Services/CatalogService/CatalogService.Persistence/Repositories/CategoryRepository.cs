using CatalogService.Application.Repositories;
using CatalogService.Application.Repositories.Contexts;
using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects;
using MongoDB.Driver;

namespace CatalogService.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IMongoDbContext _context;

        public CategoryRepository(IMongoDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Categories.Find(_ => true).ToListAsync();
        }

        public async Task<Category> GetCategoryByCategoryId(CategoryId categoryId)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.Id, categoryId.Id);
            return await _context.Categories.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.Id, category.Id);

            await _context.Categories.ReplaceOneAsync(
                  _context.Session,
                  filter,
                  category,
                  new ReplaceOptions { IsUpsert = false }
              );
        }
    }
}
