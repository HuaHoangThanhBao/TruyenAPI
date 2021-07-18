using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLibrary.Models
{
    [Table("binh_luan")]
    public class BinhLuan
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BinhLuanID { get; set; }

        //Tập khóa ngoại
        [ForeignKey(nameof(User))]
        [Required(ErrorMessage = "UserID không được để trống")]
        public Guid UserID { get; set; }
        public User User { get; set; }

        //Tập khóa ngoại
        [ForeignKey(nameof(Chuong))]
        [Required(ErrorMessage = "ChuongID không được để trống")]
        public int ChuongID { get; set; }
        public Chuong Chuong { get; set; }

        [Required(ErrorMessage = "Nội dung bình luận không được để trống")]
        [StringLength(500, ErrorMessage = "Nội dung bình luận không được vượt quá 500 ký tự")]
        public string NoiDung { get; set; }

        public string NgayBL { get; set; }

        [DefaultValue(false)]
        public bool TinhTrang { get; set; }
    }
}
