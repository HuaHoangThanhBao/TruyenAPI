using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.DataTransferObjects
{
    public class AuthResponseDto
    {
        public bool IsAuthSuccessful { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public bool Is2StepVerificationRequired { get; set; }
        public string Provider { get; set; }
    }
}
