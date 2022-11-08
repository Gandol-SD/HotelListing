using HotelListing.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListing.API.Data
{
    public class ApiDBX : IdentityDbContext<HotelUser>
    {
        public ApiDBX(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Sudan",
                    ShortName = "SD"
                },
                new Country
                {
                    Id = 2,
                    Name = "Jamaica",
                    ShortName = "JM"
                },
                new Country
                {
                    Id = 3,
                    Name = "Egypt",
                    ShortName = "EG"
                },
                new Country
                {
                    Id = 4,
                    Name = "Rwanda",
                    ShortName = "RW"
                },
                new Country
                {
                    Id = 5,
                    Name = "St.Lucia",
                    ShortName = "LC"
                }
            );
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Corinthia",
                    Address = "JG47+JF2, Nile Street, Al Khurtum, Sudan",
                    CountryId = 1,
                    Rating = 4.4
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Comfort Suits",
                    Address = "Goerge Town, Jamaica",
                    CountryId = 2,
                    Rating = 4.2
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Grand Palladium",
                    Address = "Cairo, Egypt",
                    CountryId = 3,
                    Rating = 4.3
                },
                new Hotel
                {
                    Id = 4,
                    Name = "Jade Mountain Resort",
                    Address = "VW7F+GG3, Mamin, St. Lucia",
                    CountryId = 5,
                    Rating = 4.7
                },
                new Hotel
                {
                    Id = 5,
                    Name = "Kigali Marriott Hotel",
                    Address = "KN 3 Ave, Kigali, Rwanda",
                    CountryId = 4,
                    Rating = 4.6
                }
            );
        }
    }
}
