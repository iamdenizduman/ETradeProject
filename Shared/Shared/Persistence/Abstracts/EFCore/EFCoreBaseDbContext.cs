using Microsoft.EntityFrameworkCore;
using Shared.Common.Interfaces;

namespace Shared.Persistence.Abstracts.EFCore
{
    public abstract class EFCoreBaseDbContext : DbContext
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        protected EFCoreBaseDbContext(DbContextOptions options, IDateTimeProvider dateTimeProvider) : base(options)
        {
            _dateTimeProvider = dateTimeProvider;
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity<Guid>>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAtUtc = _dateTimeProvider.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAtUtc = _dateTimeProvider.UtcNow;
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }

}
