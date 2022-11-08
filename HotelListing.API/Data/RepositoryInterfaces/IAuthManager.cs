using HotelListing.API.Models.DTOs.HotelUser;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Data.RepositoryInterfaces
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>> Register(HotelUserDto userDto);
    }
}
