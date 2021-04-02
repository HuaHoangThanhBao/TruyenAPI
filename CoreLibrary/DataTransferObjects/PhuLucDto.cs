using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.DataTransferObjects
{
    public class PhuLucDto
    {
        public int PhuLucID { get; set; }

        public int TruyenID { get; set; }

        public TheLoai TheLoai { get; set; }
        public int TheLoaiID { get; set; }
    }
}
