using CatalogService.Application.Repositories;
using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects;
using CatalogService.Persistence.Repositories.Contexts;
using MongoDB.Driver;

namespace CatalogService.Persistence.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly MongoDbContext _context;

        public CategoryRepository(MongoDbContext context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryByCategoryId(CategoryId categoryId)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.Id, categoryId.Id);
            return await _context.Categories.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateCategoryAsync(Category category, IClientSessionHandle session)
        {
            var filter = Builders<Category>.Filter.Eq(c => c.Id, category.Id);

            await _context.Categories.ReplaceOneAsync(
                  session,
                  filter,
                  category,
                  new ReplaceOptions { IsUpsert = false }
              );
        }
    }
}
