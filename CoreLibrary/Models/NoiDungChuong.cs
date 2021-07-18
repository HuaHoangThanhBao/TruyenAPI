using System.ComponentModel;
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

        [Required(ErrorMessage = "ChuongID không được để trống")]
        [ForeignKey(nameof(Chuong))]
        public int ChuongID { get; set; }
        public Chuong Chuong { get; set; }

        [Required(ErrorMessage = "Đường dẫn hình ảnh không được để trống")]
        public string HinhAnh { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
