using CatalogService.Domain.Aggregates.ProductAggregate.Exceptions;

namespace CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects
{
    public record CategoryId
    {
        public Guid Value { get; }
        public CategoryId(Guid value)
        {
            if (value == Guid.Empty)
                throw new ProductDomainExcepiton("CategoryId boş olamaz.");
            Value = value;
        }
    }
}
