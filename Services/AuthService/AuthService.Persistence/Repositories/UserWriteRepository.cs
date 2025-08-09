using AuthService.Application.Repositories.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.Abstracts.EFCore;

namespace AuthService.Persistence.Repositories
{
    public class UserWriteRepository : EFCoreWriteRepository<User>, IUserWriteRepository
    {
        public UserWriteRepository(AuthDbContext context) : base(context)
        {
        }
    }
}
