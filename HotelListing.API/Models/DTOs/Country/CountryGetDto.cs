using HotelListing.API.Models.DTOs.Hotel;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListing.API.Models.DTOs.Country
{
    public class CountryGetDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }
    }
}
