using AuthService.Infrastructure.Redis.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Common.Interfaces;
using Shared.Infrastructure.Redis;
using Shared.Infrastructure.Redis.Models;

namespace AuthService.Infrastructure.Redis
{
    public class UserRedisService(IOptions<RedisOptions> options, IDateTimeProvider provider) 
        : RedisService(options, provider), IUserRedisService
    {
        public async Task<bool> IsExistRefreshTokenByEmail(string email)
        {
            var refreshToken = await GetAsync($"refreshToken:{email}");
            return !string.IsNullOrEmpty(refreshToken);
        }
    }
}
