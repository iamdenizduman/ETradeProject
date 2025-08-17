using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Persistence.Configurations
{
    public class UserOperationClaimConfiguration : IEntityTypeConfiguration<UserOperationClaim>
    {
        public void Configure(EntityTypeBuilder<UserOperationClaim> builder)
        {
            builder.ToTable("UserOperationClaims");

            builder.HasKey(uoc => uoc.Id);

            builder.Property(uoc => uoc.UserId)
                   .IsRequired();

            builder.Property(uoc => uoc.OperationClaimId)
                   .IsRequired();

            builder.HasOne(uoc => uoc.User)
                   .WithMany(u => u.UserOperationClaims)
                   .HasForeignKey(uoc => uoc.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(uoc => uoc.OperationClaim)
                   .WithMany(oc => oc.UserOperationClaims)
                   .HasForeignKey(uoc => uoc.OperationClaimId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
