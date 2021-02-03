using CoreLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Configuration
{
    public class TheoDoiConfiguration : IEntityTypeConfiguration<TheoDoi>
    {
        public void Configure(EntityTypeBuilder<TheoDoi> builder)
        {
            builder.ToTable("theo_doi");

            //n - 1 relations
            builder.HasOne(e => e.Truyen)
                .WithMany(s => s.TheoDois)
                .HasForeignKey(s => s.TruyenID)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.User)
                .WithMany(s => s.TheoDois)
                .HasForeignKey(s => s.UserID)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
