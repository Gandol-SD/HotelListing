using AutoMapper;
using HotelListing.API.Data.RepositoryInterfaces;
using HotelListing.API.Models;
using HotelListing.API.Models.DTOs.HotelUser;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace HotelListing.API.Data.Repositories
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper mapper;
        private readonly UserManager<HotelUser> userManager;
        private readonly IConfiguration config;

        public AuthManager(IMapper _mapper, UserManager<HotelUser> _userManager, IConfiguration _config)
        {
            mapper = _mapper;
            userManager = _userManager;
            config = _config;
        }

        public async Task<AuthResponseDto> login(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await userManager.CheckPasswordAsync(user, loginDto.Password);

            if (user == null || isValidUser == false)
            {
                return null;
            }

            var token = await generateToken(user);
            return new AuthResponseDto { token = token, uid = user.Id };
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

        public async Task<string> generateToken(HotelUser hotelUser)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await userManager.GetRolesAsync(hotelUser);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            var userClaims = await userManager.GetClaimsAsync(hotelUser);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, hotelUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, hotelUser.Email),
                new Claim("uid", hotelUser.Id),

            }.Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                    issuer: config["JwtSettings:Issuer"],
                    audience: config["JwtSettings:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(Convert.ToInt32(config["JwtSettings:DurationInMinutes"])),
                    signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
