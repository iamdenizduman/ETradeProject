using CatalogService.Domain.Abstracts;

namespace CatalogService.Domain.Aggregates.CategoryAggregate.Exceptions
{
    public class CategoryDomainException : BaseDomainException
    {
        public CategoryDomainException() { }

        public CategoryDomainException(string message) : base(message) { }

        public CategoryDomainException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
