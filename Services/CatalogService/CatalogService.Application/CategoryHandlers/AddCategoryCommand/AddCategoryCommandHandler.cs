using CatalogService.Application.Repositories;
using CatalogService.Application.Results;
using CatalogService.Domain.Aggregates.CategoryAggregate;
using MediatR;

namespace CatalogService.Application.CategoryHandlers.AddCategoryCommand
{
    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryRequest, DataResult<AddCategoryResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public AddCategoryCommandHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<DataResult<AddCategoryResponse>> Handle(AddCategoryRequest request, CancellationToken cancellationToken)
        {
            var category = new Category(request.Name);
            _unitOfWork.TrackEntity(category);

            await _unitOfWork.CategoryRepository.AddCategoryAsync(category);       
            return DataResult<AddCategoryResponse>.Success(new AddCategoryResponse(category.Id, category.Name), "Category added successfully");
        }
    }
}
