using CatalogService.Application.Repositories;
using MediatR;

namespace CatalogService.Application.Common.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public TransactionBehavior(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_unitOfWork.HasActiveTransaction)
                return await next(cancellationToken);

            await _unitOfWork.StartSessionAndTransactionAsync(cancellationToken);

            try
            {
                var response = await next(cancellationToken);

                var domainEntities = _unitOfWork.GetTrackedEntitiesWithEvents();
                var domainEvents = domainEntities
                    .SelectMany(e => e.DomainEvents)
                    .ToList();

                foreach (var domainEvent in domainEvents)
                    await _mediator.Publish(domainEvent, cancellationToken);

                foreach (var entity in domainEntities)
                    entity.ClearDomainEvents();

                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return response;
            }
            catch
            {
                await _unitOfWork.AbortTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                _unitOfWork.Dispose();
            }
        }
    }

}
