using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class BinhLuanForCreationDto
    {
        [Required(ErrorMessage = "UserID không được để trống")]
        public Guid UserID { get; set; }

        public int? ChuongID { get; set; }

        public int? TruyenID { get; set; }

        [Required(ErrorMessage = "Nội dung bình luận không được để trống")]
        [StringLength(500, ErrorMessage = "Nội dung bình luận không được vượt quá 500 ký tự")]
        public string NoiDung { get; set; }

        [DefaultValue(5)]
        [Range(1, 5)]
        public int DanhGiaSao { get; set; }

        public string NgayBL { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }

    public class BinhLuanForUpdateDto
    {
        [Required(ErrorMessage = "UserID không được để trống")]
        public Guid UserID { get; set; }

        public int? ChuongID { get; set; }

        public int? TruyenID { get; set; }

        [Required(ErrorMessage = "Nội dung bình luận không được để trống")]
        [StringLength(500, ErrorMessage = "Nội dung bình luận không được vượt quá 500 ký tự")]
        public string NoiDung { get; set; }

        [DefaultValue(5)]
        [Range(1, 5)]
        public int DanhGiaSao { get; set; }

        public string NgayBL { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
