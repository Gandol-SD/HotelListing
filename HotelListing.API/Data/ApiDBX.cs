using HotelListing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
    public class ApiDBX : DbContext
    {
        public ApiDBX(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }
    }
}
