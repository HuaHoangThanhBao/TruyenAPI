using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreLibrary.Models
{
    [Table("phu_luc")]
    public class PhuLuc
    {
        [Key]
        public int PhuLucID { get; set; }

        [Required(ErrorMessage = "TruyenID is required")]
        //Tập khóa ngoại
        [ForeignKey(nameof(Truyen))]
        public int TruyenID { get; set; }
        public Truyen Truyen { get; set; }

        [Required(ErrorMessage = "TheLoaiID is required")]
        //Tập khóa ngoại
        [ForeignKey(nameof(TheLoai))]
        public int TheLoaiID { get; set; }
        public TheLoai TheLoai { get; set; }
    }
}
