using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.Instructor;
using KarateClub.Application.Handlers.Instructor.Queries;
using KarateClub.Application.Handlers.Person.Queries;
using KarateClub.Application.Handlers.Person;
using KarateClub.Application.Handlers.User.Queries;
using KarateClub.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Handlers.Instructor.Commands;

namespace KarateClub.Api.Controllers.Instructor
{
    [Route("api/instructors")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly GetInstructorHandler _getInstructorHandler;
        private readonly GetInstructorsHandler _getInstructorsHandler;
        private readonly DeactivateInstructorHandler _deactivateInstructorHandler;
        private readonly ActivateInstructorHandler _activateInstructorHandler;

        public InstructorsController(GetInstructorHandler getInstructorHandler, GetInstructorsHandler getInstructorsHandler, DeactivateInstructorHandler deactivateInstructorHandler, ActivateInstructorHandler activateInstructorHandler)
        {
            _getInstructorHandler = getInstructorHandler;
            _getInstructorsHandler = getInstructorsHandler;
            _deactivateInstructorHandler = deactivateInstructorHandler;
            _activateInstructorHandler = activateInstructorHandler;
        }

        [Authorize(Policy = Permissions.Instructors.View)]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorDetailsDto>> GetInstructorById(int id)
        {
            return Ok(await _getInstructorHandler.ExecuteAsync(new GetInstructorQuery(id)));
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InstructorDto>>> GetPeople()
        {
            return Ok(await _getInstructorsHandler.ExecuteAsync(new GetInstructorsQuery()));
        }

        [Authorize(Policy = Permissions.Instructors.Delete)]
        [HttpPatch("{id:int}/deactivate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeactivateInstructor(int id)
        {
            await _deactivateInstructorHandler.ExecuteAsync(new DeactivateInstructorCommand(id));
            return NoContent();
        }

        [Authorize(Policy = Permissions.Instructors.Update)]
        [HttpPatch("{id:int}/activate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ActivateInstructor(int id)
        {
            await _activateInstructorHandler.ExecuteAsync(new ActivateInstructorCommand(id));
            return NoContent();
        }
    }
}
