using CatalogService.Domain.Aggregates.ProductAggregate.Exceptions;

namespace CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects
{
    public record Money
    {
        public decimal Amount { get; private set; }
        public string Currency { get; private set; }
        public Money(decimal amount, string currency)
        {
            if (amount < 0)
            {
                throw new ProductDomainExcepiton("Fiyat negatif olamaz.");
            }
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new ProductDomainExcepiton("Para birimi boş olamaz.");
            }

            Amount = amount;
            Currency = currency;
        }
    }
}
