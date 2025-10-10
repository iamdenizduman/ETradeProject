using CatalogService.Domain.Abstracts;
using CatalogService.Domain.Aggregates.ProductAggregate.Exceptions;
using CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects;

namespace CatalogService.Domain.Aggregates.ProductAggregate
{
    public class Product : BaseAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Money Money { get; private set; }
        public CategoryId CategoryId { get; private set; }
        public Product(string name, string description, Money money, CategoryId categoryId)
        {
            ValidateProductName(name);
            Name = name;
            ValidateProductDescription(description);
            Description = description;
            Money = money;
            CategoryId = categoryId;
        }
        public void Update(string name, string description, Money money)
        {
            ValidateProductName(name);
            Name = name;
            ValidateProductDescription(description);
            Description = description;
            Money = money;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdateName(string name)
        {
            ValidateProductName(name);
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdateDescription(string description)
        {
            ValidateProductDescription(description);
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdatePrice(Money money)
        {
            Money = money;
            UpdatedAt = DateTime.UtcNow;
        }
        public void UpdateCategory(CategoryId categoryId)
        {
            CategoryId = categoryId;
            UpdatedAt = DateTime.UtcNow;
        }
        public void Deactivate()
        {
            RecordStatus = false;
            UpdatedAt = DateTime.UtcNow;
        }
        public void Activate()
        {
            RecordStatus = true;
            UpdatedAt = DateTime.UtcNow;
        }
        private static void ValidateProductName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ProductDomainExcepiton("Product name cannot be empty.");

            if (name.Length < 3)
                throw new ProductDomainExcepiton("Product name must be at least 3 characters.");
        }
        private static void ValidateProductDescription(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ProductDomainExcepiton("Product description cannot be empty.");

            if (name.Length < 3)
                throw new ProductDomainExcepiton("Product description must be at least 3 characters.");
        }
    }
}
