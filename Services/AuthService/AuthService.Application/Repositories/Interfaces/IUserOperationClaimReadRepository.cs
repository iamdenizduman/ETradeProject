using AuthService.Domain.Entities;
using Shared.Persistence.Interfaces.EFCore;

namespace AuthService.Application.Repositories.Interfaces
{
    public interface IUserOperationClaimReadRepository : IEFCoreReadRepository<UserOperationClaim>
    {
    }
}
