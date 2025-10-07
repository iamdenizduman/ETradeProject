using MediatR;

namespace CatalogService.Domain.Abstracts
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<INotification> DomainEvents { get; }
        void ClearDomainEvents();
    }
}
