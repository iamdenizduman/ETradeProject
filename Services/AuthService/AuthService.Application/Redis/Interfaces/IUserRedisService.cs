using Shared.Infrastructure.Redis.Interfaces;

namespace AuthService.Application.Redis.Interfaces
{
    public interface IUserRedisService : IRedisService
    {
        Task<string> GetEmailByRefreshToken(string refreshToken);
    }
}
