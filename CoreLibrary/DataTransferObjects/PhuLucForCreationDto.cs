﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class PhuLucForCreationDto
    {
        [Required(ErrorMessage = "TruyenID is required")]
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "TheLoaiID is required")]
        public int TheLoaiID { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }

    public class PhuLucForUpdateDto
    {
        [Required(ErrorMessage = "TruyenID is required")]
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "TheLoaiID is required")]
        public int TheLoaiID { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
