using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Persistence.Configurations
{
    public class OperationClaimConfiguration : IEntityTypeConfiguration<OperationClaim>
    {
        public void Configure(EntityTypeBuilder<OperationClaim> builder)
        {
            builder.ToTable("OperationClaims");

            builder.HasKey(oc => oc.Id);

            builder.Property(oc => oc.Role)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasMany(oc => oc.UserOperationClaims)
                   .WithOne(uoc => uoc.OperationClaim)
                   .HasForeignKey(uoc => uoc.OperationClaimId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
