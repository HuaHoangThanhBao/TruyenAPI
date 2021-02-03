using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            builder.Property(s => s.TenUser)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.Password)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(s => s.TinhTrang)
                .HasDefaultValue(false);

            //n - 1 relations
            builder.HasMany(e => e.BinhLuans)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.BinhLuanID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.TheoDois)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.TheoDoiID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
