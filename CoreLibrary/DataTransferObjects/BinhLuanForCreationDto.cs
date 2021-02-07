using System;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class BinhLuanForCreationDto
    {
        [Required(ErrorMessage = "UserID is required")]
        public Guid UserID { get; set; }

        [Required(ErrorMessage = "TruyenID is required")]
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "Noi dung is required")]
        public string NoiDung { get; set; }

        [Required(ErrorMessage = "Ngay binh luan is required")]
        public DateTime NgayBL { get; set; }
    }

    public class BinhLuanForUpdateDto
    {
        [Required(ErrorMessage = "UserID is required")]
        public Guid UserID { get; set; }

        [Required(ErrorMessage = "TruyenID is required")]
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "Noi dung is required")]
        public string NoiDung { get; set; }

        [Required(ErrorMessage = "Ngay binh luan is required")]
        public DateTime NgayBL { get; set; }
    }
}
