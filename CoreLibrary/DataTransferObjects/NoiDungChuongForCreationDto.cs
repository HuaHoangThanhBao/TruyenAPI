using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class NoiDungChuongForCreationDto
    {
        [Required(ErrorMessage = "ChuongID is required")]
        public int ChuongID { get; set; }

        [Required(ErrorMessage = "Hinh anh is required")]
        public string HinhAnh { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }

    public class NoiDungChuongForUpdateDto
    {
        [Required(ErrorMessage = "ChuongID is required")]
        public int ChuongID { get; set; }

        [Required(ErrorMessage = "Hinh anh is required")]
        public string HinhAnh { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
