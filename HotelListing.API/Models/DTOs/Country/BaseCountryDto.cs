namespace HotelListing.API.Models.DTOs.Country
{
    public abstract class BaseCountryDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }
    }
}
