namespace CatalogService.Domain.Abstracts
{
    public interface IEntity : IAuditable
    {
        string Id { get; }
    }
}
