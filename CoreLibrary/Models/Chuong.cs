using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreLibrary.Models
{
    [Table("chuong")]
    public class Chuong
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ChuongID { get; set; }


        [Required(ErrorMessage = "TruyenID is required")]
        //Tập khóa ngoại
        [ForeignKey(nameof(Truyen))]
        public int TruyenID { get; set; }
        public Truyen Truyen { get; set; }

        public ICollection<BinhLuan> BinhLuans { get; set; }
        public ICollection<NoiDungChuong> NoiDungChuongs { get; set; }


        [Required(ErrorMessage = "Tên chương is required")]
        public string TenChuong { get; set; }
        
        public string ThoiGianCapNhat { get; set; }

        [DefaultValue(0)]
        public int LuotXem { get; set; }

        //0: chờ duyệt, 1: đã duyệt
        [DefaultValue(0)]
        public int TrangThai { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
