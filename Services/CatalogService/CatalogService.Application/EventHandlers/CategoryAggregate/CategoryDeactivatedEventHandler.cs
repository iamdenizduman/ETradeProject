using CatalogService.Domain.Aggregates.CategoryAggregate.Events;
using MediatR;

namespace CatalogService.Application.EventHandlers.CategoryAggregate
{
    public class CategoryDeactivatedEventHandler :
        INotificationHandler<CategoryDeactivatedEvent>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryDeactivatedEventHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CategoryDeactivatedEvent notification, CancellationToken cancellationToken)
        {
            var products = await _unitOfWork.ProductRepository.GetProductsByCategoryId(notification.CategoryId);

            foreach (var product in products)
            {
                product.Deactivate();
                await _unitOfWork.ProductRepository.UpdateProductAsync(product);
            }
        }
    }
}
