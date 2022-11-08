using HotelListing.API.Data.RepositoryInterfaces;
using HotelListing.API.Models.DTOs.HotelUser;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager auth;

        public AccountController(IAuthManager _auth)
        {
            auth = _auth;
        }

        // Post: api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] HotelUserDto userDto)
        {
            var err = await auth.Register(userDto);
            if (err.Any())
            {
                foreach (var error in err)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok();
        }

        // Post: api/Account/login
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] LoginDto loginDto)
        {
            var isValidUser = await auth.Login(loginDto);
            if (!isValidUser) { return Unauthorized(); }

            return Ok();
        }

    }
}
