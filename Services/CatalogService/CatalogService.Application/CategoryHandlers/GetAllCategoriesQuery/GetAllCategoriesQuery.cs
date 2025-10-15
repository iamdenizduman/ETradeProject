using CatalogService.Application.Results;
using MediatR;

namespace CatalogService.Application.CategoryHandlers.GetAllCategoriesQuery
{
    public record GetAllCategoriesQuery() : IRequest<DataResult<List<CategoryDto>>>;
}
