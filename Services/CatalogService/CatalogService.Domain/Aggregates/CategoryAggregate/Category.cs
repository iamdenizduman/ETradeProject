using CatalogService.Domain.Abstracts;
using CatalogService.Domain.Aggregates.CategoryAggregate.Events;
using CatalogService.Domain.Aggregates.CategoryAggregate.Exceptions;
using CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects;

namespace CatalogService.Domain.Aggregates.CategoryAggregate
{
    public class Category : BaseAggregateRoot
    {
        public string Name { get; private set; }
        public Category(string name) 
        {
            ValidateCategoryName(name);
            Name = name;
        }
        public void UpdateName(string name)
        {
            ValidateCategoryName(name);
            Name = name;
            UpdatedAt = DateTime.UtcNow;            
        }
        public void Deactivate()
        {
            RecordStatus = false;
            UpdatedAt = DateTime.UtcNow;
            AddDomainEvent(new CategoryDeactivatedEvent()
            {
                CategoryId = new CategoryId(Id)
            });
        }
        public void Activate()
        {
            RecordStatus = true;
            UpdatedAt = DateTime.UtcNow;
        }
        private static void ValidateCategoryName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new CategoryDomainException("Category name cannot be empty.");

            if (name.Length < 3)
                throw new CategoryDomainException("Category name must be at least 3 characters.");
        }
    }
}
