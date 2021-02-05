using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreLibrary.DataTransferObjects
{
    public class NoiDungTruyenForCreationDto
    {
        public int TruyenID { get; set; }

        public IFormFile HinhAnh { get; set; }
    }

    public class NoiDungTruyenForUpdateDto
    {
        [Required(ErrorMessage = "ID NoiDung is required")]
        public int NoiDungTruyenID { get; set; }

        [Required(ErrorMessage = "ID Truyen is required")]
        public int TruyenID { get; set; }

        public IFormFile HinhAnh { get; set; }
    }
}
