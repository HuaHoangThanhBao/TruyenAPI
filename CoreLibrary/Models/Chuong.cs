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
        public Guid ChuongID { get; set; }


        //Tập khóa ngoại
        public Guid TruyenID { get; set; }
        public Truyen Truyen { get; set; }


        public string TenChuong { get; set; }
        public DateTime ThoiGianCapNhat { get; set; }
        public int LuotXem { get; set; }
    }
}
