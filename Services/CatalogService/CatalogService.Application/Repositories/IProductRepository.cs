using CatalogService.Domain.Aggregates.ProductAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects;
using MongoDB.Driver;

namespace CatalogService.Application.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryId(CategoryId categoryId);
        Task UpdateProductAsync(Product product);
    }
}
