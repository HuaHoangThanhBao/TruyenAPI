using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.DataTransferObjects
{
    public class TheLoaiDto
    {
        public int TheLoaiID { get; set; }

        public string TenTheLoai { get; set; }

        public bool TinhTrang { get; set; }

        public IEnumerable<PhuLucDto> PhuLucs { get; set; }
    }
}
