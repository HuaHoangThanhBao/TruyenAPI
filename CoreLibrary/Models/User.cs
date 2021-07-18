using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreLibrary.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserID { get; set; }

        public string Username { get; set; }

        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(50, ErrorMessage = "Tên không được vượt quá 50 ký tự")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Họ không được để trống")]
        [StringLength(50, ErrorMessage = "Họ không được vượt quá 50 ký tự")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [StringLength(30, ErrorMessage = "Email không được vượt quá 30 ký tự")]
        public string Email { get; set; }

        [DefaultValue(0)]
        public int Quyen { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        public string Password { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        //Tập khóa ngoại
        public ICollection<TheoDoi> TheoDois { get; set; }
        public ICollection<BinhLuan> BinhLuans { get; set; }
    }
}
