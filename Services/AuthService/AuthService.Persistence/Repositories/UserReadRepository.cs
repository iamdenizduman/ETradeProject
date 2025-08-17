using AuthService.Application.Repositories.Interfaces;
using AuthService.Domain.Entities;
using AuthService.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Shared.Persistence.Abstracts.EFCore;

namespace AuthService.Persistence.Repositories
{
    public class UserReadRepository : EFCoreReadRepository<User>, IUserReadRepository
    {
        private readonly DbSet<User> _dbSet;
        public UserReadRepository(AuthDbContext context) : base(context)
        {
            _dbSet = context.Set<User>();
        }

        public async Task<User?> GetUserRoleByEmailAsync(string email)
        {
            var user = await _dbSet.Where(u => u.Email == email).Include(uoc => uoc.UserOperationClaims)
                .ThenInclude(oc => oc.OperationClaim).FirstOrDefaultAsync();

            return user;
        }
    }
}
