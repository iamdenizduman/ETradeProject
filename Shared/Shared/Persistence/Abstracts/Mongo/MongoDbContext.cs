using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Shared.Persistence.Interfaces.Mongo;
using Shared.Persistence.Models;

namespace Shared.Persistence.Abstracts.Mongo
{
    public abstract class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
