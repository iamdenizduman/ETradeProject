using CatalogService.Application.Repositories;
using CatalogService.Application.Results;
using MediatR;

namespace CatalogService.Application.CategoryHandlers.GetAllCategoriesQuery
{
    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, DataResult<List<CategoryDto>>>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly ICategoryRepository _categoryRepository;

        public GetAllCategoriesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = _unitOfWork.CategoryRepository;
        }

        public async Task<DataResult<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetCategories();
            var categoryDtos = categories.Select(c => new CategoryDto
                                                                            (c.Id,
                                                                                c.Name,
                                                                                c.RecordStatus,
                                                                                c.CreatedAt,
                                                                               c.UpdatedAt
                                                                            )).ToList();

            return DataResult<List<CategoryDto>>.Success(categoryDtos);
        }
    }
}
