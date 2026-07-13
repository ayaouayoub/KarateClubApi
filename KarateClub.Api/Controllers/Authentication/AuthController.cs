using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Handlers.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KarateClub.Api.Controllers.Authentication.Requests;

namespace KarateClub.Api.Controllers.Authentication
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LoginHandler _login;

        public AuthController(LoginHandler login)
        {
            _login = login;
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequest Request)
        {
            return Ok(await _login.ExecuteAsync(new LoginCommand
            (
                Request.Username,
                Request.Password
            )));
        }
    }
}
