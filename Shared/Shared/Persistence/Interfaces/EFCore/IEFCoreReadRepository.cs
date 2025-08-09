namespace Shared.Persistence.Interfaces.EFCore
{
    public interface IEFCoreReadRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(Guid id);
        Task<List<TEntity>> GetAllAsync();
    }
}
