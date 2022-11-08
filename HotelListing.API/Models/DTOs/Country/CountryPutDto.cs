using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.DTOs.Country
{
    public class CountryPutDto : BaseCountryDto
    {
        [Required]
        public int Id { get; set; }
    }
}
