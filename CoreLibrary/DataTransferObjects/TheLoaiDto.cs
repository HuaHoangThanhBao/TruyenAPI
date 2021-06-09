using System.Collections.Generic;

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
