using System;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class ChuongForCreationDto
    {
        public int TruyenID { get; set; }


        [Required(ErrorMessage = "Tên chương is required")]
        public string TenChuong { get; set; }

        [Required(ErrorMessage = "Thời gian cập nhật is required")]
        public DateTime ThoiGianCapNhat { get; set; }

        public int LuotXem { get; set; }
    }

    public class ChuongForUpdateDto
    {
        public int TruyenID { get; set; }


        [Required(ErrorMessage = "Tên chương is required")]
        public string TenChuong { get; set; }

        [Required(ErrorMessage = "Thời gian cập nhật is required")]
        public DateTime ThoiGianCapNhat { get; set; }

        public int LuotXem { get; set; }
    }
}
