using CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects;
using MediatR;

namespace CatalogService.Domain.Aggregates.CategoryAggregate.Events
{
    public record CategoryDeactivatedEvent : INotification
    {
        public CategoryId CategoryId { get; init; }
    }

}
