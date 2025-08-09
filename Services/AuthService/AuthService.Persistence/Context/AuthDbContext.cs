using AuthService.Domain.Entities;
using AuthService.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Shared.Auth.Enums;
using Shared.Common.Interfaces;
using Shared.Persistence.Abstracts.EFCore;

namespace AuthService.Persistence.Context
{
    public class AuthDbContext : EFCoreBaseDbContext
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        public AuthDbContext(DbContextOptions<AuthDbContext> options, IDateTimeProvider dateTimeProvider) 
            : base(options, dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
        public DbSet<OperationClaim> OperationClaims { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new OperationClaimConfiguration());
            modelBuilder.ApplyConfiguration(new UserOperationClaimConfiguration());

            var operationClaims = Enum.GetValues(typeof(RoleType))
                .Cast<RoleType>()
                .Where(role => role != RoleType.None)
                .Select(role => new OperationClaim
                {
                    Id = Guid.NewGuid(), 
                    Role = role.ToString(),
                    CreatedAtUtc = _dateTimeProvider.UtcNow
                });

            modelBuilder.Entity<OperationClaim>().HasData(operationClaims.ToArray());
        }
    }
}
