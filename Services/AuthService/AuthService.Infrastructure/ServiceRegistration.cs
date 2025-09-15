using AuthService.Application.Redis.Interfaces;
using AuthService.Infrastructure.Redis;
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
