using HotelListing.API.Models.DTOs.Hotel;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.Models.DTOs.Country
{
    public class CountryGetDto : BaseCountryDto
    {
        public int Id { get; set; }
    }
}
