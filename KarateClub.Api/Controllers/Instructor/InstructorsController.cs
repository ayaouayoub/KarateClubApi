using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.Instructor;
using KarateClub.Application.Handlers.Instructor.Queries;
using KarateClub.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using KarateClub.Application.Handlers.Instructor.Commands;
using KarateClub.Api.Controllers.Instructor.Requests;
using KarateClub.Api.Controllers.Person.Requests;
using KarateClub.Application.Handlers.Person.Commnds;
using KarateClub.Application.Handlers.Person;

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
        private readonly UpdateCurrentBletRankHandler _updateCurrentBletRankHandler;
        private readonly CreateInstructorHandler _createInstructorHandler;

        public InstructorsController(GetInstructorHandler getInstructorHandler, GetInstructorsHandler getInstructorsHandler, DeactivateInstructorHandler deactivateInstructorHandler, ActivateInstructorHandler activateInstructorHandler, UpdateCurrentBletRankHandler updateCurrentBletRankHandler, CreateInstructorHandler createInstructorHandler)
        {
            _getInstructorHandler = getInstructorHandler;
            _getInstructorsHandler = getInstructorsHandler;
            _deactivateInstructorHandler = deactivateInstructorHandler;
            _activateInstructorHandler = activateInstructorHandler;
            _updateCurrentBletRankHandler = updateCurrentBletRankHandler;
            _createInstructorHandler = createInstructorHandler;
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

        [Authorize (Policy = Permissions.Instructors.View)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<InstructorDto>>> GetInstructors()
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

        [Authorize (Policy = Permissions.Instructors.Update)]
        [HttpPatch("{id:int}/blet-rank")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCurrentBletRank(int id, [FromBody] UpdateCurrentBletRankRequest request)
        {
            await _updateCurrentBletRankHandler.ExecuteAsync(new UpdateCurrentBletRankCommand(id, request.BeltRank));
            return NoContent();
        }

        [Authorize (Policy = Permissions.Instructors.Create)]
        [HttpPost]
        [ProducesResponseType(typeof(InstructorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorDto>> CreateInstructor([FromBody] CreateInstructorRequest request)
        {
            var command = new CreateInstructorCommand
            {
                BeltRankID = request.BeltRankID,
                PersonId = request.PersonId,
                Qualification = request.Qualification,
            };

            InstructorDto instructor = await _createInstructorHandler.ExecuteAsync(command);

            return CreatedAtAction(nameof(GetInstructorById), new { id = instructor.Id }, instructor);
        }
    }
}
