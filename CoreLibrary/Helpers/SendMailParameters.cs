using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Helpers
{
    public class SendMailParameters: QueryStringParameters
    {
        public string Address { get; set; }
    }
}
