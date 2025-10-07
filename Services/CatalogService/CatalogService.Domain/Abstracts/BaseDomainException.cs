namespace CatalogService.Domain.Abstracts
{
    public class BaseDomainException : Exception
    {
        protected BaseDomainException() { }
        protected BaseDomainException(string message) : base(message) { }
        protected BaseDomainException(string message, Exception inner) : base(message, inner) { }
    }
}
