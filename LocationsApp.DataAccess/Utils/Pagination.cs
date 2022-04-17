namespace LocationsApp.DataAccess.Utils
{
    public class Pagination
    {
        private const int _minPageSize = 50;
        private const int _maxPageSize = 500;
        private int _pageNumber = 1;
        private int _pageSize = 50;

        public Pagination() { }

        public Pagination(int pageNumber)
        {
            _pageNumber = pageNumber;
        }

        public Pagination(int pageNumber, int pageSize)
        {
            _pageNumber = pageNumber;
            _pageSize = pageSize;
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = (value <= 0) ? 1 : value; }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value <= 0)
                {
                    _pageSize = _minPageSize;
                    return;
                }
                if (value > _maxPageSize)
                {
                    _pageSize = _maxPageSize;
                    return;
                }
                _pageSize = value;
            }
        }

        public int Skip
        {
            get { return (_pageNumber - 1) * _pageSize; }
        }

        public int Take
        {
            get { return PageSize; }
        }

        public int Offset
        {
            get { return (_pageNumber - 1) * _pageSize; }
        }
    }
}
