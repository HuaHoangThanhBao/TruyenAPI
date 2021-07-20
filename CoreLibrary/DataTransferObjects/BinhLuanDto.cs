using CoreLibrary.Models;
using System;

namespace CoreLibrary.DataTransferObjects
{
    public class BinhLuanDto
    {
        public int BinhLuanID { get; set; }

        public Guid UserID { get; set; }

        public User User { get; set; }

        public int? ChuongID { get; set; }

        public Chuong Chuong { get; set; }

        public int? TruyenID { get; set; }
        
        public Truyen Truyen { get; set; }

        public string NoiDung { get; set; }

        public DateTime NgayBL { get; set; }

        public bool TinhTrang { get; set; }
    }
}
