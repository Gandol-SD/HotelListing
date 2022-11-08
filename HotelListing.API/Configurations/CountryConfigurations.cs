using HotelListing.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListing.API.Configurations
{
    public class CountryConfigurations : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            
            builder.HasData(
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
        }
    }
}
