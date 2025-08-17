using Shared.Infrastructure.Redis.Interfaces;

namespace AuthService.Infrastructure.Redis.Interfaces
{
    public interface IUserRedisService : IRedisService
    {
        Task<bool> IsExistRefreshTokenByEmail(string email);
    }
}
