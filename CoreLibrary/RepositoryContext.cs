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

        public DbSet<Truyen> Truyens { get; set; }
        public DbSet<NoiDungChuong> NoiDungChuongs { get; set; }
        public DbSet<TacGia> TacGias { get; set; }
        public DbSet<TheLoai> TheLoais { get; set; }
        public DbSet<Chuong> Chuongs { get; set; }
        public DbSet<PhuLuc> PhuLucs { get; set; }
        public DbSet<TheoDoi> TheoDois { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BinhLuan> BinhLuans { get; set; }
    }
}
