using CoreLibrary.Models;
using System.Collections.Generic;

namespace CoreLibrary.DataTransferObjects
{
    public class TacGiaDto
    {
        public int TacGiaID { get; set; }

        public string TenTacGia { get; set; }

        public bool TinhTrang { get; set; }

        public IEnumerable<Truyen> Truyens { get; set; }
    }
}
