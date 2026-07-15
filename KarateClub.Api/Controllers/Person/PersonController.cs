using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.Person;
using KarateClub.Application.Handlers.Person.Queries;
using KarateClub.Application.Handlers.User;
using KarateClub.Application.Handlers.User.Queries;
using KarateClub.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        public PersonController(
            GetPesronHandler getPesront, 
            GetUserByPersonIdHandler getUserByPersonIdHandler,
            GetPeopleHandler getPeopleHandler)
        {
            _getPesront = getPesront;
            _getUserByPersonIdHandler = getUserByPersonIdHandler;
            _getPeopleHandler = getPeopleHandler;
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

        [Authorize]
        [HttpGet("/api/persons/{personId:int}/user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserWithoutPermissionsDto>> GetUserByPersonId(int personId)
        {
            return Ok(await _getUserByPersonIdHandler.ExecuteAsync(new GetUserByPersonIdQuery(personId)));
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<PersonDto>>> GetUserByPersonId()
        {
            return Ok(await _getPeopleHandler.ExecuteAsync(new GetPeopleQuery()));
        }
    }
}
