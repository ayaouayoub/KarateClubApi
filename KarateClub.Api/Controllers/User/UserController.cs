using KarateClub.Api.Controllers.User.Requests;
using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.User;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Handlers.User.Queries;
using KarateClub.Application.Security;
using KarateClub.Domain.Entities;
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
        private readonly AddUserHandler _addUserHandler;
        private readonly GetUserHandler _getUserHandler;


        public UserController
            (
            GetCurrentUserHandler current, 
            GetUsersHandler users, 
            DeactivateUserHandler deactivateUser, 
            AddUserHandler addUserHandler,
            GetUserHandler getUserHandler)
        {
            _current = current;
            _users = users;
            _deactivateUser = deactivateUser;
            _addUserHandler = addUserHandler;
            _getUserHandler = getUserHandler;
        }

        [Authorize(Policy = Permissions.Users.View)]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserDto>> GetUsers()
        {
            return Ok(await _users.ExecuteAsync());
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        [Authorize(Policy = Permissions.Users.Create)]
        [HttpPost]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> AddUser([FromBody] CreateUserRequest request)
        {
            var command = new CreateUserCommand
            {
                Username = request.Username,
                Password = request.Password,
                PersonId = request.PersonId,
                Permissions = request.Permissions?.Select(p => Permission.Load(p.Id, p.Code)).ToList()
            };

            UserDto user = await _addUserHandler.ExecuteAsync(command);

            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDetialsDto>> GetUserById(int id)
        {
            return Ok(await _getUserHandler.ExecuteAsync(new GetUserByIdQuery(id)));
        }
    }
}
