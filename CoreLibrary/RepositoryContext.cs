using CoreLibrary.Models;
using Entities.Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new TruyenConfiguration());
            //modelBuilder.ApplyConfiguration(new TheLoaiConfiguration());
            //modelBuilder.ApplyConfiguration(new TacGiaConfiguration());
            //modelBuilder.ApplyConfiguration(new UserConfiguration());
            //modelBuilder.ApplyConfiguration(new BinhLuanConfiguration());
            //modelBuilder.ApplyConfiguration(new TheoDoiConfiguration());
            //modelBuilder.ApplyConfiguration(new ChuongConfiguration());
            //modelBuilder.ApplyConfiguration(new PhuLucConfiguration());
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
