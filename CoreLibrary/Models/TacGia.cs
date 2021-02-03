using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreLibrary.Models
{
    [Table("tac_gia")]
    public class TacGia
    {
        [Key]
        public Guid TacGiaID { get; set; }
        
        [Required(ErrorMessage = "Ten tac gia is required")]
        [StringLength(50, ErrorMessage = "Ten tac gia can't be longer than 50 characters")]
        public string TenTacGia { get; set; }
        
        [DefaultValue(false)]
        public bool TinhTrang { get; set; }

        //Khóa ngoại dẫn tới bảng truyện (1 tác giả thuộc nhiều truyện)
        public ICollection<Truyen> Truyens { get; set; }
    }
}
