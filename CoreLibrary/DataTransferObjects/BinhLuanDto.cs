using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.DataTransferObjects
{
    public class BinhLuanDto
    {
        public int BinhLuanID { get; set; }

        public Guid UserID { get; set; }

        public int ChuongID { get; set; }

        public string NoiDung { get; set; }

        public DateTime NgayBL { get; set; }
    }
}
