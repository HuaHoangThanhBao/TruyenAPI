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
        public int TruyenID { get; set; }

        //Tập khóa ngoại
        public int TacGiaID { get; set; }
        public TacGia TacGia { get; set; }


        public ICollection<NoiDungTruyen> NoiDungTruyens { get; set; }
        public ICollection<PhuLuc> PhuLucs { get; set; }
        public ICollection<Chuong> Chuongs { get; set; }
        public ICollection<TheoDoi> TheoDois { get; set; }
        public ICollection<BinhLuan> BinhLuans { get; set; }


        [Required(ErrorMessage = "Ten truyen is required")]
        [StringLength(50, ErrorMessage = "Ten truyen can't be longer than 50 characters")]
        public string TenTruyen { get; set; }

        [Required(ErrorMessage = "Mo ta is required")]
        public string MoTa { get; set; }

        [DefaultValue(0)]
        public int LuotXem { get; set; }
        
        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
