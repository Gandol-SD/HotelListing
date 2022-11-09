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
        private readonly ILogger<AccountController> logger;

        public AccountController(IAuthManager _auth, ILogger<AccountController> logger)
        {
            auth = _auth;
            this.logger = logger;
        }

        // Post: api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Register([FromBody] HotelUserDto userDto)
        {
            logger.LogInformation($"Register Attempt for {userDto.Email}");
            try
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
            catch (Exception ex)
            {
                logger.LogError(ex, $"Exeception At {typeof(Register)}, by {userDto.Email}");
                return Problem(ex.Message, statusCode:500);
            }
        }

        // Post: api/Account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {
            var authResponse = await auth.login(loginDto);
            if (authResponse == null) { return Unauthorized(); }

            return Ok(authResponse);
        }

        // Post: api/Account/refresToken
        [HttpPost]
        [Route("refreshToken")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> refreshToken([FromBody] AuthResponseDto request)
        {
            var authResponse = await auth.VerifyRefreshToken(request);
            if(authResponse == null) { return Unauthorized(); }
            return Ok(authResponse);
        }

    }
}
