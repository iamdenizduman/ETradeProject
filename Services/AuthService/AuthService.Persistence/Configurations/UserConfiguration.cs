﻿using AuthService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AuthService.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(u => u.PasswordSalt)
                   .IsRequired();

            builder.Property(u => u.PasswordHash)
                   .IsRequired();

            builder.HasMany(u => u.UserOperationClaims)
                   .WithOne()
                   .HasForeignKey(uoc => uoc.UserId);
        }
    }
}
