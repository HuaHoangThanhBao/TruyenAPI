using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class UserForCreationDto
    {
        [Required(ErrorMessage = "Ten user is required")]
        [StringLength(50, ErrorMessage = "Ten user can't be longer than 50 characters")]
        public string TenUser { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Password can't be longer than 50 characters")]
        public string Password { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }

    public class UserForUpdateDto
    {
        [Required(ErrorMessage = "Ten user is required")]
        [StringLength(50, ErrorMessage = "Ten user can't be longer than 50 characters")]
        public string TenUser { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Password can't be longer than 50 characters")]
        public string Password { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
