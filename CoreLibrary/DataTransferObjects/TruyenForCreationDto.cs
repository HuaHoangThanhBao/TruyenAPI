using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class TruyenForCreationDto
    {
        public int TacGiaID { get; set; }

        [Required(ErrorMessage = "Ten truyen is required")]
        [StringLength(50, ErrorMessage = "Ten truyen can't be longer than 50 characters")]
        public string TenTruyen { get; set; }

        [Required(ErrorMessage = "Mo ta is required")]
        public string MoTa { get; set; }

        [DefaultValue(0)]
        public int LuotXem { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        [Required(ErrorMessage = "Hinh anh is required")]
        public string HinhAnh { get; set; }
    }

    public class TruyenForUpdateDto
    {
        public int TacGiaID { get; set; }

        [Required(ErrorMessage = "Ten truyen is required")]
        [StringLength(50, ErrorMessage = "Ten truyen can't be longer than 50 characters")]
        public string TenTruyen { get; set; }

        [Required(ErrorMessage = "Mo ta is required")]
        public string MoTa { get; set; }

        [DefaultValue(0)]
        public int LuotXem { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        [Required(ErrorMessage = "Hinh anh is required")]
        public string HinhAnh { get; set; }
    }
}
