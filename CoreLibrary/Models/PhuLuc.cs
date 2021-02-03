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
        public Guid PhuLucID { get; set; }

        //Tập khóa ngoại
        public Guid TruyenID { get; set; }
        public Truyen Truyen { get; set; }

        //Tập khóa ngoại
        public Guid TheLoaiID { get; set; }
        public TheLoai TheLoai { get; set; }
    }
}
