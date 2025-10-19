using CatalogService.Application.Results;
using MediatR;

namespace CatalogService.Application.CategoryHandlers.DeactivateCategoryCommand
{
    public class DeactivateCategoryRequest : IRequest<Result>
    {
        public string CategoryId { get; }
        public DeactivateCategoryRequest(string categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
