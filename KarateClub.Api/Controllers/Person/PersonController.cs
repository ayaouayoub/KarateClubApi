using KarateClub.Api.Controllers.Person.Requests;
using KarateClub.Api.Controllers.User.Requests;
using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.Instructor;
using KarateClub.Application.Handlers.Instructor.Queries;
using KarateClub.Application.Handlers.Person;
using KarateClub.Application.Handlers.Person.Commnds;
using KarateClub.Application.Handlers.Person.Queries;
using KarateClub.Application.Handlers.User;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Handlers.User.Queries;
using KarateClub.Application.Security;
using KarateClub.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KarateClub.Api.Controllers.Person
{
    [Route("api/persons")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly GetPesronHandler _getPesront;
        private readonly GetUserByPersonIdHandler _getUserByPersonIdHandler;
        private readonly GetPeopleHandler _getPeopleHandler;
        private readonly AddNewPersonHandler _addNewPersonHandler;
        private readonly UpdatePersonHandler _updatePersonHandler;
        private readonly GetInstructorByPersonIdHandler _getInstructorByPersonIdHandler;

        public PersonController(
            GetPesronHandler getPesront,
            GetUserByPersonIdHandler getUserByPersonIdHandler,
            GetPeopleHandler getPeopleHandler,
            AddNewPersonHandler addNewPersonHandler,
            UpdatePersonHandler updatePersonHandler,
            GetInstructorByPersonIdHandler getInstructorByPersonIdHandler)
        {
            _getPesront = getPesront;
            _getUserByPersonIdHandler = getUserByPersonIdHandler;
            _getPeopleHandler = getPeopleHandler;
            _addNewPersonHandler = addNewPersonHandler;
            _updatePersonHandler = updatePersonHandler;
            _getInstructorByPersonIdHandler = getInstructorByPersonIdHandler;
        }

        [Authorize]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<PersonDto>> GetPersonById(int id)
        {
            return Ok(await _getPesront.ExecuteAsync(new GetPesonbByIdQuery(id)));
        }

        [Authorize(Policy = Permissions.Users.View)]
        [HttpGet("/api/persons/{personId:int}/user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<InstructorDto>> GetUserByPersonId(int personId)
        {
            return Ok(await _getUserByPersonIdHandler.ExecuteAsync(new GetUserByPersonIdQuery(personId)));
        }

        [Authorize(Policy = Permissions.Instructors.View)]
        [HttpGet("/api/persons/{personId:int}/instructor")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserWithoutPermissionsDto>> GetInstructorByPersonId(int personId)
        {
            return Ok(await _getInstructorByPersonIdHandler.ExecuteAsync(new GetInstructorByPersonIdQuery(personId)));
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PersonDto>>> GetPeople()
        {
            return Ok(await _getPeopleHandler.ExecuteAsync(new GetPeopleQuery()));
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(PersonDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonDto>> AddNewPerson([FromBody] CreatePersonRequest request)
        {
            var command = new CreatePersonCommand
            {
                Name = request.Name,
                Address = request.Address,
                Email = request.Email
            };

            PersonDto person = await _addNewPersonHandler.ExecuteAsync(command);

            return CreatedAtAction(nameof(GetPersonById), new { id = person.Id }, person);
        }

        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<PersonDto>> UpdatePerson(int id, [FromBody] UpdatePersonRequest request)
        {
            var command = new UpdatePersonCommand
            {
                PersonId = id,
                Name = request.Name,
                Address = request.Address,
                Email = request.Email
            };

            PersonDto person = await _updatePersonHandler.ExecuteAsync(command);

            return Ok(person);
        }

    }
}
