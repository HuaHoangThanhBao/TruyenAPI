using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class BinhLuanConfiguration : IEntityTypeConfiguration<BinhLuan>
    {
        public void Configure(EntityTypeBuilder<BinhLuan> builder)
        {
            builder.ToTable("binh_luan");

            builder.Property(s => s.NoiDung)
                .IsRequired();

            builder.Property(s => s.NgayBL)
                .IsRequired();

            //n - 1 relations
            builder.HasOne(e => e.Chuong)
                .WithMany(s => s.BinhLuans)
                .HasForeignKey(s => s.BinhLuanID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.User)
                .WithMany(s => s.BinhLuans)
                .HasForeignKey(s => s.UserID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
