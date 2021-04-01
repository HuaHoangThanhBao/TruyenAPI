using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.DataTransferObjects
{
    public class BinhLuanDto
    {
        public int BinhLuanID { get; set; }

        public Guid UserID { get; set; }

        public User User { get; set; }

        public int ChuongID { get; set; }

        public Chuong Chuong { get; set; }

        public string NoiDung { get; set; }

        public DateTime NgayBL { get; set; }
    }
}
