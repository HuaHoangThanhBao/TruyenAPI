using CoreLibrary.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary
{
    public class RepositoryContext : IdentityDbContext<ApplicationUser>
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Truyen> Truyens { get; set; }
        public virtual DbSet<NoiDungChuong> NoiDungChuongs { get; set; }
        public virtual DbSet<TacGia> TacGias { get; set; }
        public virtual DbSet<TheLoai> TheLoais { get; set; }
        public virtual DbSet<Chuong> Chuongs { get; set; }
        public virtual DbSet<PhuLuc> PhuLucs { get; set; }
        public virtual DbSet<TheoDoi> TheoDois { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<BinhLuan> BinhLuans { get; set; }
    }
}
