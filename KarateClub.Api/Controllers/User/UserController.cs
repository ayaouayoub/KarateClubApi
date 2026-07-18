using KarateClub.Api.Controllers.User.Requests;
using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.User;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Handlers.User.Queries;
using KarateClub.Application.Security;
using KarateClub.Domain.Entities;
using KarateClub.Infrastructure.Authorization;
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
        private readonly UpdateUserHandler _updateUserHandler;
        private readonly ChangeMyUsernameHandler _changeMyUsernameHandler;
        private readonly ChangeMyPasswordHandler _changeMyPasswordHandler;

        public UserController
            (
            GetCurrentUserHandler current,
            GetUsersHandler users,
            DeactivateUserHandler deactivateUser,
            AddUserHandler addUserHandler,
            GetUserHandler getUserHandler,
            UpdateUserHandler updateUserHandler,
            ChangeMyUsernameHandler changeMyUsernameHandler,
            ChangeMyPasswordHandler changeMyPasswordHandler)
        {
            _current = current;
            _users = users;
            _deactivateUser = deactivateUser;
            _addUserHandler = addUserHandler;
            _getUserHandler = getUserHandler;
            _updateUserHandler = updateUserHandler;
            _changeMyUsernameHandler = changeMyUsernameHandler;
            _changeMyPasswordHandler = changeMyPasswordHandler;
        }

        [Authorize(Policy = Permissions.Users.View)]
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
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
        public ActionResult<UserDetialsDto> Me()
        {
            return Ok(_current.Execute());
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

        [Authorize(Policy = Permissions.Users.View)]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDetialsDto>> GetUserById(int id)
        {
            return Ok(await _getUserHandler.ExecuteAsync(new GetUserByIdQuery(id)));
        }


        [Authorize(Policy = Permissions.Users.Update)]
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            var command = new UpdateUserCommand
            {
                UserId = id,
                Username = request.Username,
                Password = request.Password,
                IsActive = request.IsActive,
                PersonId = request.PersonId,
                Permissions = request.Permissions?.Select(p => Permission.Load(p.Id, p.Code)).ToList()
            };

            UserDto user = await _updateUserHandler.ExecuteAsync(command);

            return Ok(user);
        }

        [Authorize]
        [HttpPatch("me/username")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangenMyUsername(ChangeMyUsernameRequest request)
        {
            await _changeMyUsernameHandler.ExecuteAsync(new UpdateMyUsernameCommand(request.Username));
            return NoContent();
        }

        [Authorize]
        [HttpPatch("me/password")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangenMyPassword(ChangeMyPasswordRequest request)
        {
            await _changeMyPasswordHandler.ExecuteAsync(new ChangeMyPasswordCommand(request.CurrentPassword, request.NewPassword));
            return NoContent();
        }
    }
}
