using Microsoft.Build.Framework;

namespace HotelListing.API.Models.DTOs
{
    public class CountryDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }
    }
}
