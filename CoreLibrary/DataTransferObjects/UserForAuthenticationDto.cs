using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class UserForAuthenticationDto
    {
        [Required(ErrorMessage = "Email không được để trống")]
        [StringLength(30, ErrorMessage = "Email không được vượt quá 30 ký tự")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }

        public string clientURI { get; set; }
    }
}
