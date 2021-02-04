using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreLibrary.Models
{
    [Table("the_loai")]
    public class TheLoai
    {
        [Key]
        public int TheLoaiID { get; set; }

        [Required(ErrorMessage = "Ten the loai is required")]
        [StringLength(50, ErrorMessage = "Ten the loai can't be longer than 50 characters")]
        public string TenTheLoai { get; set; }

        [Required(ErrorMessage = "Tinh trang is required")]
        public bool TinhTrang { get; set; }

        public ICollection<PhuLuc> PhuLucs { get; set; }
    }
}
