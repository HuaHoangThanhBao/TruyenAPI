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

        [Required(ErrorMessage = "Tên thể loại không được để trống")]
        [StringLength(100, ErrorMessage = "Tên thể loại không được vượt quá 100 ký tự")]
        public string TenTheLoai { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        public ICollection<PhuLuc> PhuLucs { get; set; }
    }
}
