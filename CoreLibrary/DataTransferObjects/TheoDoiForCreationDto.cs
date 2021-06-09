using System;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class TheoDoiForCreationDto
    {
        [Required(ErrorMessage = "TruyenID is required")]
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "UserID is required")]
        public Guid UserID { get; set; }
    }

    public class TheoDoiForUpdateDto
    {
        [Required(ErrorMessage = "TruyenID is required")]
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "UserID is required")]
        public Guid UserID { get; set; }
    }
}
