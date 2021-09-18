using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace CoreLibrary.DataTransferObjects
{
    public class UserForAcceptanceDto
    {
        public Guid UserID { get; set; }

        [DefaultValue(0)]
        public int Quyen { get; set; }
    }
}
