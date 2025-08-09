using AuthService.Application.Repositories.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.Abstracts.EFCore;

namespace AuthService.Persistence.Repositories
{
    public class UserOperationClaimReadRepository : EFCoreReadRepository<UserOperationClaim>, IUserOperationClaimReadRepository
    {
        public UserOperationClaimReadRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
