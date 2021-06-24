using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class TacGiaForCreationDto
    {
        [Required(ErrorMessage = "Ten tac gia is required")]
        public string TenTacGia { get; set; }

        [Required(ErrorMessage = "Tinh trang is required")]
        public bool TinhTrang { get; set; }
    }

    public class TacGiaForUpdateDto
    {
        [Required(ErrorMessage = "Ten tac gia is required")]
        public string TenTacGia { get; set; }

        [Required(ErrorMessage = "Tinh trang is required")]
        public bool TinhTrang { get; set; }
    }
}
