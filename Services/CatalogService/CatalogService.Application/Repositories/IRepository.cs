using CatalogService.Domain.Abstracts;

namespace CatalogService.Application.Repositories
{
    public interface IRepository<T>
        where T : IAggregateRoot
    {
    }
}
