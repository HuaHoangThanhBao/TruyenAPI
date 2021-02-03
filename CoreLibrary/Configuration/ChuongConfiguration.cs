using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class ChuongConfiguration : IEntityTypeConfiguration<Chuong>
    {
        public void Configure(EntityTypeBuilder<Chuong> builder)
        {
            builder.ToTable("chuong");

            builder.Property(s => s.TenChuong)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.LuotXem)
                .HasDefaultValue(0);

            //n - 1 relations
            builder.HasOne(e => e.Truyen)
                .WithMany(s => s.Chuongs)
                .HasForeignKey(s => s.TruyenID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
