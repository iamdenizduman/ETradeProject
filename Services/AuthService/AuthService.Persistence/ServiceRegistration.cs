using AuthService.Application.Repositories.Interfaces;
using AuthService.Persistence.Context;
using AuthService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Persistence;
using Shared.Persistence.Interfaces.EFCore;

namespace AuthService.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services, string sqlConnection)
        {
            services.AddDbContext<AuthDbContext>(opt =>
            {
                opt.UseSqlServer(sqlConnection);
            });

            #region Repository IOC
            services.AddScoped<IOperationClaimReadRepository, OperationClaimReadRepository>();
            services.AddScoped<IOperationClaimWriteRepository, OperationClaimWriteRepository>();
            services.AddScoped<IUserOperationClaimReadRepository, UserOperationClaimReadRepository>();
            services.AddScoped<IUserOperationClaimWriteRepository, UserOperationClaimWriteRepository>();
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork<AuthDbContext>>();
            #endregion
        }
    }
}
