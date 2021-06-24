namespace CoreLibrary.Helpers
{
    public abstract class QueryStringParameters
    {
        const int maxPageSize = 500;
        public int PageNumber { get; set; } = 1;
        private int _pageSize = 20;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }

        private string _apiKey;
        public string APIKey
        {
            get
            {
                return _apiKey;
            }
            set
            {
                _apiKey = value;
            }
        }
    }
}
