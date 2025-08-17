using Microsoft.Extensions.Options;
using Shared.Common.Interfaces;
using Shared.Infrastructure.Redis.Interfaces;
using Shared.Infrastructure.Redis.Models;
using StackExchange.Redis;

namespace Shared.Infrastructure.Redis
{
    public class RedisService : IRedisService
    {
        protected readonly IDatabase _db;
        protected IDateTimeProvider _dateTimeProvider;

        public RedisService(IOptions<RedisOptions> options, IDateTimeProvider dateTimeProvider)
        {
            var redis = ConnectionMultiplexer.Connect(options.Value.ConnectionString);
            _db = redis.GetDatabase();
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
            => await _db.StringSetAsync(key, value, expiry);

        public async Task<string?> GetAsync(string key)
        {
            var value = await _db.StringGetAsync(key);
            return value.HasValue ? value.ToString() : null;
        }

        public async Task RemoveAsync(string key)
            => await _db.KeyDeleteAsync(key);
    }
}
