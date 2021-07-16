using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLibrary.Models
{
    [Table("the_loai")]
    public class TheLoai
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TheLoaiID { get; set; }

        [Required(ErrorMessage = "Ten the loai is required")]
        [StringLength(50, ErrorMessage = "Ten the loai can't be longer than 50 characters")]
        public string TenTheLoai { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        public ICollection<PhuLuc> PhuLucs { get; set; }
    }
}
