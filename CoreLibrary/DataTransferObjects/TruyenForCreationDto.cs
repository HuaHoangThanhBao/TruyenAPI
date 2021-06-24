﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class TruyenForCreationDto
    {
        [Required(ErrorMessage = "TacGiaID is required")]
        public int TacGiaID { get; set; }

        [Required(ErrorMessage = "Ten truyen is required")]
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
        [Required(ErrorMessage = "TacGiaID is required")]
        public int TacGiaID { get; set; }

        [Required(ErrorMessage = "Ten truyen is required")]
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
