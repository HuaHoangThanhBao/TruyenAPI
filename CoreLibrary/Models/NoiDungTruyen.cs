using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLibrary.Models
{
    [Table("noi_dung_truyen")]
    public class NoiDungTruyen
    {
        [Key]
        public int NoiDungTruyenID { get; set; }

        [Required(ErrorMessage = "ID Truyen is required")]
        [ForeignKey(nameof(Truyen))]
        public int TruyenID { get; set; }
        public Truyen Truyen { get; set; }

        [Required(ErrorMessage = "HinhAnh is required")]
        public string HinhAnh { get; set; }
    }
}
