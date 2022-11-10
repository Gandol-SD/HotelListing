namespace HotelListing.API.Models
{
    public class QueryParams
    {
        private int _pageSize = 15;
        public int StartIndex { get; set; }
        public int pageNumber { get; set; }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
    }
}
