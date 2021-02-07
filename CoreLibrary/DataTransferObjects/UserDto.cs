using CoreLibrary.Models;
using System;
using System.Collections.Generic;

namespace CoreLibrary.DataTransferObjects
{
    public class UserDto
    {
        public Guid UserID { get; set; }

        public string TenUser { get; set; }

        public string Password { get; set; }

        public bool TinhTrang { get; set; }

        public IEnumerable<TheoDoi> TheoDois { get; set; }
        public IEnumerable<BinhLuan> BinhLuans { get; set; }
    }
}
