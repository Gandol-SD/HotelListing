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
        private HotelUser _user;

        public AuthManager(IMapper _mapper, UserManager<HotelUser> _userManager, IConfiguration _config)
        {
            mapper = _mapper;
            userManager = _userManager;
            config = _config;
        }

        public async Task<AuthResponseDto> login(LoginDto loginDto)
        {
            _user = await userManager.FindByEmailAsync(loginDto.Email);
            bool isValidUser = await userManager.CheckPasswordAsync(_user, loginDto.Password);

            if (_user == null || isValidUser == false)
            {
                return null;
            }

            var token = await generateToken();
            return new AuthResponseDto { token = token, uid = _user.Id, RefreshToken = await createRefreshToken() };
        }

        public async Task<IEnumerable<IdentityError>> Register(HotelUserDto userDto)
        {
            _user = mapper.Map<HotelUser>(userDto);
            _user.UserName = userDto.Email;

            var result = await userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(_user, "User"); // what about Admins ???
            }

            return result.Errors;
        }

        public async Task<string> generateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var roles = await userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            var userClaims = await userManager.GetClaimsAsync(_user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id),

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

        public async Task<string> createRefreshToken()
        {
            await userManager.RemoveAuthenticationTokenAsync(_user, "HotelListingApi", "RefreshToken");
            var newRefreshToken = await userManager.GenerateUserTokenAsync(_user, "HotelListingApi", "RefreshToken");
            var result = userManager.SetAuthenticationTokenAsync(_user, "HotelListingApi", "RefreshToken", newRefreshToken);
            return newRefreshToken.ToString();
        }

        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var jwtSecTokHandler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecTokHandler.ReadJwtToken(request.token);
            var userName = tokenContent.Claims.ToList().FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email)?.Value;
            _user = await userManager.FindByNameAsync(userName);

            if(_user == null || _user.Id != request.uid) { return null; }
            var isValidToken = await userManager.VerifyUserTokenAsync(_user, "HotelListingApi", "RefreshToken", request.RefreshToken);

            if(isValidToken)
            {
                var token = await generateToken();
                return new AuthResponseDto
                {
                    token = token,
                    uid = _user.Id,
                    RefreshToken = await createRefreshToken()
                };
            }
            await userManager.UpdateSecurityStampAsync(_user);
            return null;
        }
    }
}
