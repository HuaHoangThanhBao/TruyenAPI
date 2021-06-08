using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class NoiDungChuongForCreationDto
    {
        [Required(ErrorMessage = "ID NoiDung is required")]
        public int ChuongID { get; set; }

        [Required(ErrorMessage = "Hinh anh is required")]
        public string HinhAnh { get; set; }
    }

    public class NoiDungChuongForUpdateDto
    {
        [Required(ErrorMessage = "ID NoiDung is required")]
        public int NoiDungChuongID { get; set; }

        [Required(ErrorMessage = "ID Truyen is required")]
        public int ChuongID { get; set; }

        [Required(ErrorMessage = "Hinh anh is required")]
        public string HinhAnh { get; set; }
    }
}
