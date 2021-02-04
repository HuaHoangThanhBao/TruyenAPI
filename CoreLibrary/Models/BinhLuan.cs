using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreLibrary.Models
{
    [Table("binh_luan")]
    public class BinhLuan
    {
        [Key]
        public int BinhLuanID { get; set; }

        //Tập khóa ngoại
        public Guid UserID { get; set; }
        public User User { get; set; }

        //Tập khóa ngoại
        public int TruyenID { get; set; }
        public Truyen Truyen { get; set; }

        [Required(ErrorMessage = "Noi dung is required")]
        public string NoiDung { get; set; }

        [Required(ErrorMessage = "Ngay binh luan is required")]
        public DateTime NgayBL { get; set; }
    }
}
