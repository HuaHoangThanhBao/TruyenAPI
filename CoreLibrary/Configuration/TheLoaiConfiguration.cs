using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class TheLoaiConfiguration : IEntityTypeConfiguration<TheLoai>
    {
        public void Configure(EntityTypeBuilder<TheLoai> builder)
        {
            builder.ToTable("the_loai");

            builder.Property(s => s.TenTheLoai)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.TinhTrang)
                .HasDefaultValue(false);

            //n - 1 relations
            builder.HasMany(e => e.PhuLucs)
                .WithOne(s => s.TheLoai)
                .HasForeignKey(s => s.PhuLucID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
