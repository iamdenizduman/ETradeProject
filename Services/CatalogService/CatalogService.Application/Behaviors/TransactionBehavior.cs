using CatalogService.Application.Repositories;
using MediatR;

namespace CatalogService.Application.Common.Behaviors
{
    public class TransactionBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TransactionBehavior(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (_unitOfWork.HasActiveTransaction)
                return await next();

            await _unitOfWork.StartSessionAndTransactionAsync(cancellationToken);

            try
            {
                var response = await next();

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
