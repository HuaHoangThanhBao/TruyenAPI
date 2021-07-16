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

        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name can't be longer than 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name can't be longer than 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [DefaultValue(0)]
        public int Quyen { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        //Tập khóa ngoại
        public ICollection<TheoDoi> TheoDois { get; set; }
        public ICollection<BinhLuan> BinhLuans { get; set; }
    }
}
