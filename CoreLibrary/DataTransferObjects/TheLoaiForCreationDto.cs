using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class TheLoaiForCreationDto
    {
        [Required(ErrorMessage = "Tên thể loại không được để trống")]
        [StringLength(100, ErrorMessage = "Tên thể loại không được vượt quá 100 ký tự")]
        public string TenTheLoai { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }

    public class TheLoaiForUpdateDto
    {
        [Required(ErrorMessage = "Tên thể loại không được để trống")]
        [StringLength(100, ErrorMessage = "Tên thể loại không được vượt quá 100 ký tự")]
        public string TenTheLoai { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
