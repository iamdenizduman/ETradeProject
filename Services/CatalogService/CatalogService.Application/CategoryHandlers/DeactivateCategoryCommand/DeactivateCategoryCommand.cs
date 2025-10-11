using CatalogService.Application.Results;
using MediatR;

namespace CatalogService.Application.CategoryHandlers.DeactivateCategoryCommand
{
    public class DeactivateCategoryCommand : IRequest<Result>
    {
        public string CategoryId { get; }
        public DeactivateCategoryCommand(string categoryId)
        {
            CategoryId = categoryId;
        }
    }
}
