using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entities.Configuration
{
    public class TruyenConfiguration : IEntityTypeConfiguration<Truyen>
    {
        public void Configure(EntityTypeBuilder<Truyen> builder)
        {
            builder.ToTable("truyen");

            builder.Property(s => s.TenTruyen)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(s => s.MoTa)
                .IsRequired();

            builder.Property(s => s.LuotXem)
                .HasDefaultValue(0);
            builder.Property(s => s.TinhTrang)
                .HasDefaultValue(false);

            //builder.Property(s => s.HinhAnh)
            //    .IsRequired();

            //1 - n relations
            builder.HasOne(e => e.TacGia)
                .WithMany(s => s.Truyens)
                .HasForeignKey(s => s.TacGiaID)
                .OnDelete(DeleteBehavior.Restrict);


            //n - 1 relations
            builder.HasMany(e => e.PhuLucs)
                .WithOne(s => s.Truyen)
                .HasForeignKey(s => s.PhuLucID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Chuongs)
                .WithOne(s => s.Truyen)
                .HasForeignKey(s => s.ChuongID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.TheoDois)
                .WithOne(s => s.Truyen)
                .HasForeignKey(s => s.TheoDoiID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
