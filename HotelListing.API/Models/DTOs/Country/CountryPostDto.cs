using Microsoft.Build.Framework;

namespace HotelListing.API.Models.DTOs.Country
{
    public class CountryPostDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }
    }
}
