using AuthService.Application.Repositories.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.Abstracts.EFCore;

namespace AuthService.Persistence.Repositories
{
    public class OperationClaimReadRepository : EFCoreReadRepository<OperationClaim>, IOperationClaimReadRepository
    {
        public OperationClaimReadRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
