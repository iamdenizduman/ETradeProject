using AuthService.Application.Repositories.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.Abstracts.EFCore;

namespace AuthService.Persistence.Repositories
{
    public class OperationClaimWriteRepository : EFCoreWriteRepository<OperationClaim>, IOperationClaimWriteRepository
    {
        public OperationClaimWriteRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
