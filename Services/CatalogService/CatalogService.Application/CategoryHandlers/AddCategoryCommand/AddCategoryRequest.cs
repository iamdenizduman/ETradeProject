using CatalogService.Application.Results;
using MediatR;

namespace CatalogService.Application.CategoryHandlers.AddCategoryCommand
{
    public class AddCategoryRequest : IRequest<DataResult<AddCategoryResponse>>
    {
        public string Name { get; set; }
    }
}
