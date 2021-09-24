using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Models
{
    public class UserInfo
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string HinhAnh { get; set; }
        public ICollection<TheoDoi> TheoDois { get; set; }
        public ICollection<BinhLuan> BinhLuans { get; set; }
    }
}
