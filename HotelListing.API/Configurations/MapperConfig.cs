using AutoMapper;
using HotelListing.API.Models;
using HotelListing.API.Models.DTOs.Country;
using HotelListing.API.Models.DTOs.Hotel;

namespace HotelListing.API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CountryPostDto>().ReverseMap();
            CreateMap<Country, CountryGetDto>().ReverseMap();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<Country, CountryPutDto>().ReverseMap();

            CreateMap<Hotel, HotelDto>().ReverseMap();
            CreateMap<Hotel, HotelCreateDto>().ReverseMap();
        }
    }
}
