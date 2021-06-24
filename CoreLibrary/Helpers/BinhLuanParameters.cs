namespace CoreLibrary.Helpers
{
    public class BinhLuanParameters: QueryStringParameters
    {
        public int TruyenID { get; set; }
        public int ChuongID { get; set; }

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
