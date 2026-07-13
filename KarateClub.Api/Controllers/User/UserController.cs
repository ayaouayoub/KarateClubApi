using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.User;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarateClub.Api.Controllers.User
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GetCurrentUserHandler _current;
        private readonly GetUsersHandler _users;
        private readonly DeactivateUserHandler _deactivateUser;

        public UserController(GetCurrentUserHandler current, GetUsersHandler users, DeactivateUserHandler deactivateUser)
        {
            _current = current;
            _users = users;
            _deactivateUser = deactivateUser;
        }

        [Authorize(Policy = Permissions.Users.View)]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> GetUsers()
        {
            return Ok(await _users.ExecuteAsync());
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDetialsDto>> Me()
        {
            return Ok(await _current.ExecuteAsync());
        }

        [Authorize(Policy = Permissions.Users.Delete)]
        [HttpPatch("{id:int}/deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeactivateUser(int id)
        {
            await _deactivateUser.ExecuteAsync(new DeactivateUserCommand(id));

            return NoContent();
        }
    }
}
