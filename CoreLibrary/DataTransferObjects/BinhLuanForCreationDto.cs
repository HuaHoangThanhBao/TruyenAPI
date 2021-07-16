using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class BinhLuanForCreationDto
    {
        [Required(ErrorMessage = "UserID is required")]
        public Guid UserID { get; set; }

        [Required(ErrorMessage = "ChuongID is required")]
        public int ChuongID { get; set; }

        [Required(ErrorMessage = "Noi dung is required")]
        public string NoiDung { get; set; }

        public string NgayBL { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }

    public class BinhLuanForUpdateDto
    {
        [Required(ErrorMessage = "UserID is required")]
        public Guid UserID { get; set; }

        [Required(ErrorMessage = "ChuongID is required")]
        public int ChuongID { get; set; }

        [Required(ErrorMessage = "Noi dung is required")]
        public string NoiDung { get; set; }

        public string NgayBL { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
