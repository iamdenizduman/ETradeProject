using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects;

namespace CatalogService.Application.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> GetCategories();
        Task<Category> GetCategoryByCategoryId(CategoryId categoryId);
        Task UpdateCategoryAsync(Category category);
    }
}
