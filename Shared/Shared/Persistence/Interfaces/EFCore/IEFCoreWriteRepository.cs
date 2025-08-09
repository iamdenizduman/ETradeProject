namespace Shared.Persistence.Interfaces.EFCore
{
    public interface IEFCoreWriteRepository<TEntity> where TEntity : class
    {
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
    }
}
