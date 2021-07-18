using System;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class TheoDoiForCreationDto
    {
        [Required(ErrorMessage = "TruyenID không được để trống")]
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "UserID không được để trống")]
        public Guid UserID { get; set; }
    }

    public class TheoDoiForUpdateDto
    {
        [Required(ErrorMessage = "TruyenID không được để trống")]
        public int TruyenID { get; set; }

        [Required(ErrorMessage = "UserID không được để trống")]
        public Guid UserID { get; set; }
    }
}
