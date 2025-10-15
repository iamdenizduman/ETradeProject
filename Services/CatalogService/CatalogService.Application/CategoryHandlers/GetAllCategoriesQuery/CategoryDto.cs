namespace CatalogService.Application.CategoryHandlers.GetAllCategoriesQuery
{
    public record CategoryDto(
        string Id,
        string Name,
        bool RecordStatus,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
