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

        public string UserName { get; set; }

        public string ApplicationUserID { get; set; }

        [DefaultValue(0)]
        public int Quyen { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        public string HinhAnh { get; set; }

        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        //Tập khóa ngoại
        public ICollection<TheoDoi> TheoDois { get; set; }
        public ICollection<BinhLuan> BinhLuans { get; set; }
    }
}
