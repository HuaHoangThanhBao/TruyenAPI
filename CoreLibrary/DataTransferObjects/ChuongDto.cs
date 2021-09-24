using CoreLibrary.Models;
using System;

namespace CoreLibrary.DataTransferObjects
{
    public class ChuongDto
    {
        public Chuong Chuong { get; set; }

        public int ChuongID { get; set; }

        public int STT { get; set; }

        public int TruyenID { get; set; }

        public string TenChuong { get; set; }

        public string ThoiGianCapNhat { get; set; }

        public int LuotXem { get; set; }

        public int TrangThai { get; set; }

        public bool TinhTrang { get; set; }
    }
}
