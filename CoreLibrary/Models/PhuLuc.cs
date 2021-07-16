using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLibrary.Models
{
    [Table("phu_luc")]
    public class PhuLuc
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
