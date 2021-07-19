using CoreLibrary.Models;
using System;
using System.Collections.Generic;

namespace CoreLibrary.DataTransferObjects
{
    public class UserDto
    {
        public Guid UserID { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int Quyen { get; set; }

        public bool TinhTrang { get; set; }

        public string HinhAnh { get; set; }

        public IEnumerable<TheoDoi> TheoDois { get; set; }
        public IEnumerable<BinhLuan> BinhLuans { get; set; }
    }
}
