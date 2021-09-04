using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class NoiDungChuongForCreationDto
    {
        [Required(ErrorMessage = "ChuongID không được để trống")]
        public int ChuongID { get; set; }

        [Required(ErrorMessage = "Đường dẫn hình ảnh không được để trống")]
        public string HinhAnh { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }

    public class NoiDungChuongForUpdateDto
    {
        [Required(ErrorMessage = "ChuongID không được để trống")]
        public int ChuongID { get; set; }

        [Required(ErrorMessage = "Đường dẫn hình ảnh không được để trống")]
        public string HinhAnh { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }


    public class NoiDungChuongForDeleteDto
    {
        [Required(ErrorMessage = "NoiDungChuongID không được để trống")]
        public int NoiDungChuongID { get; set; }

        [Required(ErrorMessage = "ChuongID không được để trống")]
        public int ChuongID { get; set; }

        [Required(ErrorMessage = "Đường dẫn hình ảnh không được để trống")]
        public string HinhAnh { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
