using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class UpdateUserAvatarDto
    {
        [Required(ErrorMessage = "Email không được để trống")]
        public string Email { get; set; }

        public string HinhAnh { get; set; }
    }
}
