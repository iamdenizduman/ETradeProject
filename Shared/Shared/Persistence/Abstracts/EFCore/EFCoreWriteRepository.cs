using Microsoft.EntityFrameworkCore;
using Shared.Persistence.Interfaces.EFCore;

namespace Shared.Persistence.Abstracts.EFCore
{
    public abstract class EFCoreWriteRepository<TEntity>(DbContext context) : IEFCoreWriteRepository<TEntity>
    where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        public async Task AddAsync(TEntity entity)
            => await _dbSet.AddAsync(entity);

        public void Update(TEntity entity)
            => _dbSet.Update(entity);

        public void Remove(TEntity entity)
            => _dbSet.Remove(entity);
    }
}
