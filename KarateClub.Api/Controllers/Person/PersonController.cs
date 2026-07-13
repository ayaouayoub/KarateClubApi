using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.Person;
using KarateClub.Application.Handlers.Person.Queries;
using KarateClub.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarateClub.Api.Controllers.Person
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly GetPesronHandler _getPesront;
        public PersonController(GetPesronHandler getPesront)
        {
            _getPesront = getPesront;
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
    }
}
