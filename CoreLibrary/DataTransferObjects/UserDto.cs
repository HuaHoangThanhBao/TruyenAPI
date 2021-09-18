using CoreLibrary.Models;
using System;
using System.Collections.Generic;

namespace CoreLibrary.DataTransferObjects
{
    public class UserDto
    {
        public Guid UserID { get; set; }

        public string UserName { get; set; }

        public int Quyen { get; set; }

        public bool TinhTrang { get; set; }

        public string HinhAnh { get; set; }

        //Dùng để get user để kiểm tra phân quyền và xét duyệt truy cập dashboard (editor)
        public bool LockoutEnabled { get; set; }

        public IEnumerable<TheoDoi> TheoDois { get; set; }
        public IEnumerable<BinhLuan> BinhLuans { get; set; }
    }
}
