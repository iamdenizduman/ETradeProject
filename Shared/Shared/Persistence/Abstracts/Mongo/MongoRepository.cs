using MongoDB.Driver;
using Shared.Persistence.Interfaces.Mongo;

namespace Shared.Persistence.Abstracts.Mongo
{
    public abstract class MongoRepository<T> : IMongoRepository<T> where T : BaseEntity<string>
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDbContext context)
        {
            _collection = context.GetCollection<T>(typeof(T).Name);
        }

        public async Task<T> GetByIdAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();

        public async Task<IEnumerable<T>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task AddAsync(T entity) =>
            await _collection.InsertOneAsync(entity);

        public async Task UpdateAsync(T entity) =>
            await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);

        public async Task DeleteAsync(string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);
    }

}
