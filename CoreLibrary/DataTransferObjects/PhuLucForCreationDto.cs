using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreLibrary.DataTransferObjects
{
    public class PhuLucForCreationDto
    {
        [Required(ErrorMessage = "TruyenID is required")]
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "TheLoaiID is required")]
        public int TheLoaiID { get; set; }
    }

    public class PhuLucForUpdateDto
    {
        [Required(ErrorMessage = "TruyenID is required")]
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "TheLoaiID is required")]
        public int TheLoaiID { get; set; }
    }
}
