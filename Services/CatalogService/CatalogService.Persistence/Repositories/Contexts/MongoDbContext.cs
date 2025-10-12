using CatalogService.Application.Repositories.Contexts;
using CatalogService.Domain.Aggregates.CategoryAggregate;
using CatalogService.Domain.Aggregates.ProductAggregate;
using CatalogService.Persistence.Models.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CatalogService.Persistence.Repositories.Contexts
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IMongoClient client, IOptions<MongoDbSettings> options)
        {
            _database = client.GetDatabase(options.Value.DatabaseName);
        }

        public IMongoCollection<Category> Categories => _database.GetCollection<Category>("Categories");
        public IMongoCollection<Product> Products => _database.GetCollection<Product>("Products");
    }
}
