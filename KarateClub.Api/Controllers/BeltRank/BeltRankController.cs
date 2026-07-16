using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.BeltRank;
using KarateClub.Application.Handlers.BeltRank.Queries;
using KarateClub.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarateClub.Api.Controllers.BeltRank
{
    [Route("api/beltranks")]
    [ApiController]
    public class BeltRankController : ControllerBase
    {
        private readonly GetBeltRankHandler _getBeltRankHandler;
        private readonly GetBeltsHandler _getBeltsHandler;

        public BeltRankController(GetBeltRankHandler getBeltRankHandler, GetBeltsHandler getBeltsHandler)
        {
            _getBeltRankHandler = getBeltRankHandler;
            _getBeltsHandler = getBeltsHandler;
        }

        [Authorize(Policy = Permissions.BeltRanks.View)]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<BeltRankDto>> GetBeltRankById(int id)
        {
            return Ok(await _getBeltRankHandler.ExecuteAsync(new GetBeltRankQuery(id)));
        }

        [Authorize(Policy = Permissions.BeltRanks.View)]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<BeltRankDto>>> GetBelts()
        {
            return Ok(await _getBeltsHandler.ExecuteAsync(new GetBeltsQuery()));
        }
    }
}
