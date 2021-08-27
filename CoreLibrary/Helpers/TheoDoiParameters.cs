using System;
using System.Collections.Generic;
using System.Text;

namespace CoreLibrary.Helpers
{
    public class TheoDoiParameters: QueryStringParameters
    {
        public string UserID { get; set; }
        public int TruyenID { get; set; }

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
        private bool _lastestUpdate;
        public bool LastestUpdate
        {
            get
            {
                return _lastestUpdate;
            }
            set
            {
                _lastestUpdate = value;
            }
        }
    }
}
