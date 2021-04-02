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

        [Required(ErrorMessage = "Ten user is required")]
        [StringLength(50, ErrorMessage = "Ten user can't be longer than 50 characters")]
        public string TenUser { get; set; }

        public int Quyen { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(30, ErrorMessage = "Password can't be longer than 50 characters")]
        public string Password { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        //Tập khóa ngoại
        public ICollection<TheoDoi> TheoDois { get; set; }
        public ICollection<BinhLuan> BinhLuans { get; set; }
    }
}
