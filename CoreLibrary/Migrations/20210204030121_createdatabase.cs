using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoreLibrary.Migrations
{
    public partial class createdatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tac_gia",
                columns: table => new
                {
                    TacGiaID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTacGia = table.Column<string>(maxLength: 50, nullable: false),
                    TinhTrang = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tac_gia", x => x.TacGiaID);
                });

            migrationBuilder.CreateTable(
                name: "the_loai",
                columns: table => new
                {
                    TheLoaiID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenTheLoai = table.Column<string>(maxLength: 50, nullable: false),
                    TinhTrang = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_the_loai", x => x.TheLoaiID);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    UserID = table.Column<Guid>(nullable: false),
                    TenUser = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(maxLength: 30, nullable: false),
                    TinhTrang = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.UserID);
                });

            migrationBuilder.CreateTable(
                name: "truyen",
                columns: table => new
                {
                    TruyenID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TacGiaID = table.Column<int>(nullable: false),
                    TenTruyen = table.Column<string>(maxLength: 50, nullable: false),
                    MoTa = table.Column<string>(nullable: false),
                    LuotXem = table.Column<int>(nullable: false),
                    TinhTrang = table.Column<bool>(nullable: false),
                    HinhAnh = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_truyen", x => x.TruyenID);
                    table.ForeignKey(
                        name: "FK_truyen_tac_gia_TacGiaID",
                        column: x => x.TacGiaID,
                        principalTable: "tac_gia",
                        principalColumn: "TacGiaID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "binh_luan",
                columns: table => new
                {
                    BinhLuanID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<Guid>(nullable: false),
                    TruyenID = table.Column<int>(nullable: false),
                    NoiDung = table.Column<string>(nullable: false),
                    NgayBL = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_binh_luan", x => x.BinhLuanID);
                    table.ForeignKey(
                        name: "FK_binh_luan_truyen_TruyenID",
                        column: x => x.TruyenID,
                        principalTable: "truyen",
                        principalColumn: "TruyenID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_binh_luan_user_UserID",
                        column: x => x.UserID,
                        principalTable: "user",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chuong",
                columns: table => new
                {
                    ChuongID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TruyenID = table.Column<int>(nullable: false),
                    TenChuong = table.Column<string>(nullable: true),
                    ThoiGianCapNhat = table.Column<DateTime>(nullable: false),
                    LuotXem = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chuong", x => x.ChuongID);
                    table.ForeignKey(
                        name: "FK_chuong_truyen_TruyenID",
                        column: x => x.TruyenID,
                        principalTable: "truyen",
                        principalColumn: "TruyenID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "phu_luc",
                columns: table => new
                {
                    PhuLucID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TruyenID = table.Column<int>(nullable: false),
                    TheLoaiID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_phu_luc", x => x.PhuLucID);
                    table.ForeignKey(
                        name: "FK_phu_luc_the_loai_TheLoaiID",
                        column: x => x.TheLoaiID,
                        principalTable: "the_loai",
                        principalColumn: "TheLoaiID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_phu_luc_truyen_TruyenID",
                        column: x => x.TruyenID,
                        principalTable: "truyen",
                        principalColumn: "TruyenID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "theo_doi",
                columns: table => new
                {
                    TheoDoiID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TruyenID = table.Column<int>(nullable: false),
                    UserID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_theo_doi", x => x.TheoDoiID);
                    table.ForeignKey(
                        name: "FK_theo_doi_truyen_TruyenID",
                        column: x => x.TruyenID,
                        principalTable: "truyen",
                        principalColumn: "TruyenID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_theo_doi_user_UserID",
                        column: x => x.UserID,
                        principalTable: "user",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_binh_luan_TruyenID",
                table: "binh_luan",
                column: "TruyenID");

            migrationBuilder.CreateIndex(
                name: "IX_binh_luan_UserID",
                table: "binh_luan",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_chuong_TruyenID",
                table: "chuong",
                column: "TruyenID");

            migrationBuilder.CreateIndex(
                name: "IX_phu_luc_TheLoaiID",
                table: "phu_luc",
                column: "TheLoaiID");

            migrationBuilder.CreateIndex(
                name: "IX_phu_luc_TruyenID",
                table: "phu_luc",
                column: "TruyenID");

            migrationBuilder.CreateIndex(
                name: "IX_theo_doi_TruyenID",
                table: "theo_doi",
                column: "TruyenID");

            migrationBuilder.CreateIndex(
                name: "IX_theo_doi_UserID",
                table: "theo_doi",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_truyen_TacGiaID",
                table: "truyen",
                column: "TacGiaID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "binh_luan");

            migrationBuilder.DropTable(
                name: "chuong");

            migrationBuilder.DropTable(
                name: "phu_luc");

            migrationBuilder.DropTable(
                name: "theo_doi");

            migrationBuilder.DropTable(
                name: "the_loai");

            migrationBuilder.DropTable(
                name: "truyen");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "tac_gia");
        }
    }
}
