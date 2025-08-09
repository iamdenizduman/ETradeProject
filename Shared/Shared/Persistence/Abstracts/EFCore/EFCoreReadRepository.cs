using Microsoft.EntityFrameworkCore;
using Shared.Persistence.Interfaces.EFCore;

namespace Shared.Persistence.Abstracts.EFCore
{
    public abstract class EFCoreReadRepository<TEntity>(DbContext context) : IEFCoreReadRepository<TEntity>
    where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

        public async Task<TEntity?> GetByIdAsync(Guid id)
            => await _dbSet.FindAsync(id);

        public async Task<List<TEntity>> GetAllAsync()
            => await _dbSet.ToListAsync();
    }
}
