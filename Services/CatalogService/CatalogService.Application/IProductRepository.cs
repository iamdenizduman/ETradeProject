using CatalogService.Domain.Aggregates.ProductAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects;

namespace CatalogService.Application
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryId(CategoryId categoryId);
        Task UpdateProductAsync(Product product);
    }
}
