using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLibrary.Models
{
    [Table("noi_dung_chuong")]
    public class NoiDungChuong
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoiDungChuongID { get; set; }

        [Required(ErrorMessage = "ID Chuong is required")]
        [ForeignKey(nameof(Chuong))]
        public int ChuongID { get; set; }
        public Chuong Chuong { get; set; }

        [Required(ErrorMessage = "HinhAnh is required")]
        public string HinhAnh { get; set; }
    }
}
