using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.User;
using KarateClub.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarateClub.Api.Controllers.User
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GetCurrentUserHandler _current;
        private readonly GetUsersHandler _users;

        public UserController(GetCurrentUserHandler current, GetUsersHandler users)
        {
            _current = current;
            _users = users;
        }

        [Authorize(Policy = Permissions.Users.View)]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CurrentUserDto>> GetUsers()
        {
            return Ok(await _users.ExecuteAsync());
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CurrentUserDto>> Me()
        {
            return Ok(await _current.ExecuteAsync());
        }
    }
}
