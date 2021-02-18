﻿// <auto-generated />
using System;
using CoreLibrary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoreLibrary.Migrations
{
    [DbContext(typeof(RepositoryContext))]
    [Migration("20210218070906_createdatabase")]
    partial class createdatabase
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CoreLibrary.Models.BinhLuan", b =>
                {
                    b.Property<int>("BinhLuanID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChuongID")
                        .HasColumnType("int");

                    b.Property<DateTime>("NgayBL")
                        .HasColumnType("datetime2");

                    b.Property<string>("NoiDung")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("BinhLuanID");

                    b.HasIndex("ChuongID");

                    b.HasIndex("UserID");

                    b.ToTable("binh_luan");
                });

            modelBuilder.Entity("CoreLibrary.Models.Chuong", b =>
                {
                    b.Property<int>("ChuongID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LuotXem")
                        .HasColumnType("int");

                    b.Property<string>("TenChuong")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ThoiGianCapNhat")
                        .HasColumnType("datetime2");

                    b.Property<int>("TruyenID")
                        .HasColumnType("int");

                    b.HasKey("ChuongID");

                    b.HasIndex("TruyenID");

                    b.ToTable("chuong");
                });

            modelBuilder.Entity("CoreLibrary.Models.NoiDungTruyen", b =>
                {
                    b.Property<int>("NoiDungTruyenID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("HinhAnh")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TruyenID")
                        .HasColumnType("int");

                    b.HasKey("NoiDungTruyenID");

                    b.HasIndex("TruyenID");

                    b.ToTable("noi_dung_truyen");
                });

            modelBuilder.Entity("CoreLibrary.Models.PhuLuc", b =>
                {
                    b.Property<int>("PhuLucID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TheLoaiID")
                        .HasColumnType("int");

                    b.Property<int>("TruyenID")
                        .HasColumnType("int");

                    b.HasKey("PhuLucID");

                    b.HasIndex("TheLoaiID");

                    b.HasIndex("TruyenID");

                    b.ToTable("phu_luc");
                });

            modelBuilder.Entity("CoreLibrary.Models.TacGia", b =>
                {
                    b.Property<int>("TacGiaID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TenTacGia")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<bool>("TinhTrang")
                        .HasColumnType("bit");

                    b.HasKey("TacGiaID");

                    b.ToTable("tac_gia");
                });

            modelBuilder.Entity("CoreLibrary.Models.TheLoai", b =>
                {
                    b.Property<int>("TheLoaiID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("TenTheLoai")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<bool>("TinhTrang")
                        .HasColumnType("bit");

                    b.HasKey("TheLoaiID");

                    b.ToTable("the_loai");
                });

            modelBuilder.Entity("CoreLibrary.Models.TheoDoi", b =>
                {
                    b.Property<int>("TheoDoiID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("TruyenID")
                        .HasColumnType("int");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("TheoDoiID");

                    b.HasIndex("TruyenID");

                    b.HasIndex("UserID");

                    b.ToTable("theo_doi");
                });

            modelBuilder.Entity("CoreLibrary.Models.Truyen", b =>
                {
                    b.Property<int>("TruyenID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LuotXem")
                        .HasColumnType("int");

                    b.Property<string>("MoTa")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TacGiaID")
                        .HasColumnType("int");

                    b.Property<string>("TenTruyen")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<bool>("TinhTrang")
                        .HasColumnType("bit");

                    b.HasKey("TruyenID");

                    b.HasIndex("TacGiaID");

                    b.ToTable("truyen");
                });

            modelBuilder.Entity("CoreLibrary.Models.User", b =>
                {
                    b.Property<Guid>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(30)")
                        .HasMaxLength(30);

                    b.Property<string>("TenUser")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<bool>("TinhTrang")
                        .HasColumnType("bit");

                    b.HasKey("UserID");

                    b.ToTable("user");
                });

            modelBuilder.Entity("CoreLibrary.Models.BinhLuan", b =>
                {
                    b.HasOne("CoreLibrary.Models.Chuong", "Chuong")
                        .WithMany("BinhLuans")
                        .HasForeignKey("ChuongID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreLibrary.Models.User", "User")
                        .WithMany("BinhLuans")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreLibrary.Models.Chuong", b =>
                {
                    b.HasOne("CoreLibrary.Models.Truyen", "Truyen")
                        .WithMany("Chuongs")
                        .HasForeignKey("TruyenID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreLibrary.Models.NoiDungTruyen", b =>
                {
                    b.HasOne("CoreLibrary.Models.Truyen", "Truyen")
                        .WithMany("NoiDungTruyens")
                        .HasForeignKey("TruyenID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreLibrary.Models.PhuLuc", b =>
                {
                    b.HasOne("CoreLibrary.Models.TheLoai", "TheLoai")
                        .WithMany("PhuLucs")
                        .HasForeignKey("TheLoaiID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreLibrary.Models.Truyen", "Truyen")
                        .WithMany("PhuLucs")
                        .HasForeignKey("TruyenID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreLibrary.Models.TheoDoi", b =>
                {
                    b.HasOne("CoreLibrary.Models.Truyen", "Truyen")
                        .WithMany("TheoDois")
                        .HasForeignKey("TruyenID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoreLibrary.Models.User", "User")
                        .WithMany("TheoDois")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoreLibrary.Models.Truyen", b =>
                {
                    b.HasOne("CoreLibrary.Models.TacGia", "TacGia")
                        .WithMany("Truyens")
                        .HasForeignKey("TacGiaID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
