using CatalogService.Domain.Aggregates.ProductAggregate.Exceptions;

namespace CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects
{
    public record CategoryId
    {
        public string Id { get; }
        public CategoryId(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ProductDomainExcepiton("CategoryId boş olamaz.");
            Id = id;
        }
    }
}
