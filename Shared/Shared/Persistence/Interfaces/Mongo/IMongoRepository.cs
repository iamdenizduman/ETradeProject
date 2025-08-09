using Shared.Persistence.Abstracts;

namespace Shared.Persistence.Interfaces.Mongo
{
    public interface IMongoRepository<T> where T : BaseEntity<string>
    {
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }

}
