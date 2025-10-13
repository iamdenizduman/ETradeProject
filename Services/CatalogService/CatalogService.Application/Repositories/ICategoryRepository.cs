using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects;
using MongoDB.Driver;

namespace CatalogService.Application.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<Category> GetCategoryByCategoryId(CategoryId categoryId);
        Task UpdateCategoryAsync(Category category);
    }
}
