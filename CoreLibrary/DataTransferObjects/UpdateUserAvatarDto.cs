using System.ComponentModel.DataAnnotations;

namespace CoreLibrary.DataTransferObjects
{
    public class UpdateUserAvatarDto
    {
        [Required(ErrorMessage = "UserID không được để trống")]
        public string UserID { get; set; }

        public string HinhAnh { get; set; }
    }
}
