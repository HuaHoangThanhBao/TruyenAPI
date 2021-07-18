using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class ChuongForCreationDto
    {
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "Tên chương không được để trống")]
        [StringLength(50, ErrorMessage = "Tên chương không được vượt quá 50 ký tự")]
        public string TenChuong { get; set; }

        public string ThoiGianCapNhat { get; set; }

        [DefaultValue(0)]
        public int LuotXem { get; set; }

        [DefaultValue(0)]
        public int TrangThai { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }

    public class ChuongForUpdateDto
    {
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "Tên chương không được để trống")]
        [StringLength(50, ErrorMessage = "Tên chương không được vượt quá 50 ký tự")]
        public string TenChuong { get; set; }

        public string ThoiGianCapNhat { get; set; }

        [DefaultValue(0)]
        public int LuotXem { get; set; }

        [DefaultValue(0)]
        public int TrangThai { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
