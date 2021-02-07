using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreLibrary.DataTransferObjects
{
    public class TheoDoiForCreationDto
    {
        [Required(ErrorMessage = "ID Truyen is required")]
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "ID User is required")]
        public Guid UserID { get; set; }
    }

    public class TheoDoiForUpdateDto
    {
        [Required(ErrorMessage = "ID Truyen is required")]
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "ID User is required")]
        public Guid UserID { get; set; }
    }
}
