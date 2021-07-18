using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoreLibrary.Models
{
    [Table("tac_gia")]
    public class TacGia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TacGiaID { get; set; }
        
        [Required(ErrorMessage = "Tên tác giả không được để trống")]
        [StringLength(100, ErrorMessage = "Tên tác giả không được vượt quá 100 ký tự")]
        public string TenTacGia { get; set; }
        
        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        //Khóa ngoại dẫn tới bảng truyện (1 tác giả thuộc nhiều truyện)
        public ICollection<Truyen> Truyens { get; set; }
    }
}
