namespace CatalogService.Domain.Abstracts
{
    public interface IAuditable
    {
        DateTime CreatedAt { get; }
        DateTime? UpdatedAt { get; }
    }
}
