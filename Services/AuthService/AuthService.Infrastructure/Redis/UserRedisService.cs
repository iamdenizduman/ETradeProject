using AuthService.Application.Redis.Interfaces;
using Microsoft.Extensions.Options;
using Shared.Common.Interfaces;
using Shared.Infrastructure.Redis;
using Shared.Infrastructure.Redis.Models;

namespace AuthService.Infrastructure.Redis
{
    public class UserRedisService(IOptions<RedisOptions> options, IDateTimeProvider provider)
        : RedisService(options, provider), IUserRedisService
    {
        public async Task<string> GetEmailByRefreshToken(string refreshToken)
        {
            var email = await GetAsync($"refreshToken:{refreshToken}");
            return email;
        }
    }
}
