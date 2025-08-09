using MongoDB.Driver;

namespace Shared.Persistence.Interfaces.Mongo
{
    public interface IMongoDbContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}
