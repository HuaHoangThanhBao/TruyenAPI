using System;

namespace CoreLibrary.Helpers
{
    public class TruyenParameters: QueryStringParameters
    {
        public int TheLoaiID { get; set; }
        public Guid UserID { get; set; }

        private bool _sorting;
        public bool Sorting
        {
            get
            {
                return _sorting;
            }
            set
            {
                _sorting = value;
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

        private bool _topView;
        public bool TopView
        {
            get
            {
                return _topView;
            }
            set
            {
                _topView = value;
            }
        }

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
