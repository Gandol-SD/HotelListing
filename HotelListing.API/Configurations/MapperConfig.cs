using AutoMapper;
using HotelListing.API.Models;
using HotelListing.API.Models.DTOs;

namespace HotelListing.API.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Country, CountryDto>().ReverseMap();
        }
    }
}
