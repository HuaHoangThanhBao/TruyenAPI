using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class TruyenForCreationDto
    {
        [Required(ErrorMessage = "TacGiaID không được để trống")]
        public int TacGiaID { get; set; }

        [Required(ErrorMessage = "Tên truyện không được để trống")]
        [StringLength(200, ErrorMessage = "Tên truyện không được vượt quá 200 ký tự")]
        public string TenTruyen { get; set; }

        [Required(ErrorMessage = "Mô tả truyện không được để trống")]
        [StringLength(1000, ErrorMessage = "Mô tả truyện không được vượt quá 1000 ký tự")]
        public string MoTa { get; set; }

        [DefaultValue(0)]
        public int TrangThai { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        [Required(ErrorMessage = "Đường dẫn hình ảnh không được để trống")]
        public string HinhAnh { get; set; }
    }

    public class TruyenForUpdateDto
    {
        [Required(ErrorMessage = "TacGiaID không được để trống")]
        public int TacGiaID { get; set; }

        [Required(ErrorMessage = "Tên truyện không được để trống")]
        [StringLength(200, ErrorMessage = "Tên truyện không được vượt quá 200 ký tự")]
        public string TenTruyen { get; set; }

        [Required(ErrorMessage = "Mô tả truyện không được để trống")]
        [StringLength(1000, ErrorMessage = "Mô tả truyện không được vượt quá 1000 ký tự")]
        public string MoTa { get; set; }

        [DefaultValue(0)]
        public int TrangThai { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        [Required(ErrorMessage = "Đường dẫn hình ảnh không được để trống")]
        public string HinhAnh { get; set; }
    }
}
