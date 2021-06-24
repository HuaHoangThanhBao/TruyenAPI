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
        [ForeignKey(nameof(TacGia))]
        public int TacGiaID { get; set; }
        public TacGia TacGia { get; set; }


        public ICollection<PhuLuc> PhuLucs { get; set; }
        public ICollection<Chuong> Chuongs { get; set; }
        public ICollection<TheoDoi> TheoDois { get; set; }


        [Required(ErrorMessage = "Ten truyen is required")]
        public string TenTruyen { get; set; }

        [Required(ErrorMessage = "Mo ta is required")]
        public string MoTa { get; set; }

        public int TrangThai { get; set; }
        
        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        [Required(ErrorMessage = "HinhAnh is required")]
        public string HinhAnh { get; set; }
    }
}
