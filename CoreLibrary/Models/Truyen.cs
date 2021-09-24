using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreLibrary.Models
{
    [Table("truyen")]
    public class Truyen
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TruyenID { get; set; }

        //Tập khóa ngoại
        [Required(ErrorMessage = "TacGiaID không được để trống")]
        [ForeignKey(nameof(TacGia))]
        public int TacGiaID { get; set; }
        public TacGia TacGia { get; set; }


        public ICollection<BinhLuan> BinhLuans { get; set; }
        public ICollection<PhuLuc> PhuLucs { get; set; }
        public ICollection<Chuong> Chuongs { get; set; }
        public ICollection<TheoDoi> TheoDois { get; set; }
        

        [Required(ErrorMessage = "Tên truyện không được để trống")]
        [StringLength(200, ErrorMessage = "Tên truyện không được vượt quá 200 ký tự")]
        public string TenTruyen { get; set; }

        [StringLength(200, ErrorMessage = "Tên khác không được vượt quá 200 ký tự")]
        public string TenKhac { get; set; }

        [Required(ErrorMessage = "Mô tả truyện không được để trống")]
        [StringLength(1000, ErrorMessage = "Mô tả truyện không được vượt quá 1000 ký tự")]
        public string MoTa { get; set; }

        //0: chờ duyệt, 1: đã duyệt
        [DefaultValue(0)]
        public int TrangThai { get; set; }
        
        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        [Required(ErrorMessage = "Đường dẫn hình ảnh không được để trống")]
        public string HinhAnh { get; set; }
    }
}
