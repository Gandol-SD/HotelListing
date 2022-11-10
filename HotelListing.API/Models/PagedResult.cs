namespace HotelListing.API.Models
{
    public class PagedResult<T>
    {
        public int totalCount { get; set; }
        public int pageNumber { get; set; }
        public int recordNumber { get; set; }
        public List<T> Items { get; set; }
    }
}
