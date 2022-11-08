using AutoMapper;
using HotelListing.API.Data.RepositoryInterfaces;
using HotelListing.API.Models;
using HotelListing.API.Models.DTOs.HotelUser;
using Microsoft.AspNetCore.Identity;

namespace HotelListing.API.Data.Repositories
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper mapper;
        private readonly UserManager<HotelUser> userManager;

        public AuthManager(IMapper _mapper, UserManager<HotelUser> _userManager)
        {
            mapper = _mapper;
            userManager = _userManager;
        }

        public async Task<IEnumerable<IdentityError>> Register(HotelUserDto userDto)
        {
            var user = mapper.Map<HotelUser>(userDto);
            user.UserName = userDto.Email;

            var result = await userManager.CreateAsync(user, userDto.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "User"); // what about Admins ???
            }

            return result.Errors;
        }
    }
}
