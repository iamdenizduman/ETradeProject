using CatalogService.Domain.Abstracts;

namespace CatalogService.Domain.Aggregates.ProductAggregate.Exceptions
{
    public class ProductDomainExcepiton : BaseDomainException
    {
        public ProductDomainExcepiton() { }

        public ProductDomainExcepiton(string message) : base(message) { }

        public ProductDomainExcepiton(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
