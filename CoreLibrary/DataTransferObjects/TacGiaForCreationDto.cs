using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class TacGiaForCreationDto
    {
        [Required(ErrorMessage = "Tên tác giả không được để trống")]
        [StringLength(100, ErrorMessage = "Tên tác giả không được vượt quá 100 ký tự")]
        public string TenTacGia { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }

    public class TacGiaForUpdateDto
    {
        [Required(ErrorMessage = "Tên tác giả không được để trống")]
        [StringLength(100, ErrorMessage = "Tên tác giả không được vượt quá 100 ký tự")]
        public string TenTacGia { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
