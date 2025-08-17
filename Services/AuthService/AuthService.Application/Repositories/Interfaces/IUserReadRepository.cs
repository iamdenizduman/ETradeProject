using AuthService.Domain.Entities;
using Shared.Persistence.Interfaces.EFCore;

namespace AuthService.Application.Repositories.Interfaces
{
    public interface IUserReadRepository : IEFCoreReadRepository<User>
    {
        Task<User?> GetUserRoleByEmailAsync(string email);
    }
}
