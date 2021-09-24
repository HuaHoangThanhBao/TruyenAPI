using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Models
{
    public class TruyenOnSearch
    {
        public int TruyenID { get; set; }

        public TacGia TacGia { get; set; }
        public Chuong NewestChapter { get; set; }

        public string TenTruyen { get; set; }

        public string TenKhac { get; set; }

        public string HinhAnh { get; set; }
    }
}
