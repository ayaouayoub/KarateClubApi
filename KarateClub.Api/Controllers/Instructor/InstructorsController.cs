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

namespace KarateClub.Api.Controllers.Instructor
{
    [Route("api/instructors")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly GetInstructorHandler _getInstructorHandler;
        private readonly GetInstructorsHandler _getInstructorsHandler;

        public InstructorsController(GetInstructorHandler getInstructorHandler, GetInstructorsHandler getInstructorsHandler)
        {
            _getInstructorHandler = getInstructorHandler;
            _getInstructorsHandler = getInstructorsHandler;
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
    }
}
