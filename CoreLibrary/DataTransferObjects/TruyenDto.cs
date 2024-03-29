﻿using CoreLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.DataTransferObjects
{
    public class TruyenDto
    {
        public int TruyenID { get; set; }

        public int TacGiaID { get; set; }
        public TacGia TacGia { get; set; }

        public IEnumerable<PhuLucDto> PhuLucs { get; set; }
        public IEnumerable<Chuong> Chuongs { get; set; }
        public IEnumerable<TheoDoi> TheoDois { get; set; }
        public IEnumerable<BinhLuan> BinhLuans { get; set; }


        public string TenTruyen { get; set; }

        public string TenKhac { get; set; }

        public string MoTa { get; set; }

        public int TrangThai { get; set; }

        public bool TinhTrang { get; set; }

        public string HinhAnh { get; set; }
    }
}
