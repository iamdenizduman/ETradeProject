using AuthService.Application.Repositories.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.Abstracts.EFCore;

namespace AuthService.Persistence.Repositories
{
    public class UserOperationClaimWriteRepository : EFCoreWriteRepository<UserOperationClaim>, IUserOperationClaimWriteRepository
    {
        public UserOperationClaimWriteRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
