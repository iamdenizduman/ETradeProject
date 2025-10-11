using CatalogService.Domain.Abstracts;

namespace CatalogService.Application
{
    public interface IRepository<T>
        where T : IAggregateRoot
    {
    }
}
