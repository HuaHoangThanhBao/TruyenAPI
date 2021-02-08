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


        //Tập khóa ngoại
        [ForeignKey(nameof(Truyen))]
        public int TruyenID { get; set; }
        public Truyen Truyen { get; set; }


        [Required(ErrorMessage = "Tên chương is required")]
        public string TenChuong { get; set; }

        [Required(ErrorMessage = "Thời gian cập nhật is required")]
        public DateTime ThoiGianCapNhat { get; set; }

        public int LuotXem { get; set; }
    }
}
