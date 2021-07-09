using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Helpers
{
    public class TheoDoiParameters: QueryStringParameters
    {
        public string Username { get; set; }

        private bool _getAll;
        public bool GetAll
        {
            get
            {
                return _getAll;
            }
            set
            {
                _getAll = value;
            }
        }
    }
}
