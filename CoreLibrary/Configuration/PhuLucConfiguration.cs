using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class PhuLucConfiguration : IEntityTypeConfiguration<PhuLuc>
    {
        public void Configure(EntityTypeBuilder<PhuLuc> builder)
        {
            builder.ToTable("phu_luc");

            //n - 1 relations
            builder.HasOne(e => e.Truyen)
                .WithMany(s => s.PhuLucs)
                .HasForeignKey(s => s.TruyenID)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasOne(e => e.TheLoai)
                .WithMany(s => s.PhuLucs)
                .HasForeignKey(s => s.TheLoaiID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
