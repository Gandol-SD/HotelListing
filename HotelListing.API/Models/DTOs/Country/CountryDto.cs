using HotelListing.API.Models.DTOs.Hotel;
using Microsoft.Build.Framework;

namespace HotelListing.API.Models.DTOs.Country
{
    public class CountryDto : BaseCountryDto
    {
        public int Id { get; set; }

        public List<HotelDto> Hotels { get; set; }
    }
}
