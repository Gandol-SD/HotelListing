using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
    public class ApiDBX : DbContext
    {
        public ApiDBX(DbContextOptions options) : base(options)
        {

        }
    }
}
