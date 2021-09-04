using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.DataTransferObjects
{
    public class ExternalAuthDto
    {
        public string Email { get; set; }
        public string Provider { get; set; }
        public string IdToken { get; set; }
        public string ClientURI { get; set; }
        public int Quyen { get; set; }
        public string UserName { get; set; }
    }
}
