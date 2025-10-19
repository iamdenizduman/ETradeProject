using CatalogService.Application.Repositories;
using CatalogService.Application.Results;
using CatalogService.Domain.Aggregates.ProductAggregate.ValueObjects;
using MediatR;

namespace CatalogService.Application.CategoryHandlers.DeactivateCategoryCommand
{
    public class DeactivateCategoryCommandHandler : IRequestHandler<DeactivateCategoryRequest, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeactivateCategoryCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Result> Handle(DeactivateCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = await _unitOfWork.CategoryRepository
                .GetCategoryByCategoryId(new CategoryId(request.CategoryId));

            if (category is null)
                return Result.Failure("Category not found");

            category.Deactivate();
            await _unitOfWork.CategoryRepository.UpdateCategoryAsync(category);

            foreach (var domainEvent in category.DomainEvents)
                await _mediator.Publish(domainEvent, cancellationToken);

            category.ClearDomainEvents();

            return Result.Success("Category deactivated successfully");
        }
    }
}
