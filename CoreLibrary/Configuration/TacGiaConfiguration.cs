using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class TacGiaConfiguration : IEntityTypeConfiguration<TacGia>
    {
        public void Configure(EntityTypeBuilder<TacGia> builder)
        {
            builder.ToTable("tac_gia");

            builder.Property(s => s.TenTacGia)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.TinhTrang)
                .HasDefaultValue(false);

            //n - 1 relations
            builder.HasMany(e => e.Truyens)
                .WithOne(s => s.TacGia)
                .HasForeignKey(s => s.TacGiaID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
