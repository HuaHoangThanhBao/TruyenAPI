﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class TheLoaiForCreationDto
    {
        [Required(ErrorMessage = "Ten the loai is required")]
        [StringLength(50, ErrorMessage = "Ten the loai can't be longer than 50 characters")]
        public string TenTheLoai { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }

    public class TheLoaiForUpdateDto
    {
        [Required(ErrorMessage = "Ten the loai is required")]
        [StringLength(50, ErrorMessage = "Ten the loai can't be longer than 50 characters")]
        public string TenTheLoai { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
