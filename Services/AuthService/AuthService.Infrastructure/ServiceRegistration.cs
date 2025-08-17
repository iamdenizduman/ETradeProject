using AuthService.Infrastructure.Redis;
using AuthService.Infrastructure.Redis.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRedisService, UserRedisService>();
        }
    }
}
