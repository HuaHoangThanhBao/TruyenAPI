using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreLibrary.Models
{
    [Table("theo_doi")]
    public class TheoDoi
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TheoDoiID { get; set; }

        //Tập khóa ngoại
        [Required(ErrorMessage = "TruyenID không được để trống")]
        [ForeignKey(nameof(Truyen))]
        public int TruyenID { get; set; }
        public Truyen Truyen { get; set; }

        //Tập khóa ngoại
        [Required(ErrorMessage = "UserID không được để trống")]
        [ForeignKey(nameof(User))]
        public Guid UserID { get; set; }
        public User User { get; set; }

        public DateTime ThoiGian { get; set; }
    }
}
