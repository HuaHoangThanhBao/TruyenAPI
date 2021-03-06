﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Models
{
    public class ResponseDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
