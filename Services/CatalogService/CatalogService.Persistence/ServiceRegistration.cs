using CatalogService.Application.Repositories;
using CatalogService.Application.Repositories.Contexts;
using CatalogService.Persistence.Models.Settings;
using CatalogService.Persistence.Repositories;
using CatalogService.Persistence.Repositories.Contexts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CatalogService.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices(this IServiceCollection services)
        {
            // Registering MongoDB settings
            services.AddSingleton<IMongoClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
                return new MongoClient(settings.ConnectionString);

            });

            // Registering MongoDbContext and repositories
            services.AddScoped<IMongoDbContext, MongoDbContext>();           
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();          
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
