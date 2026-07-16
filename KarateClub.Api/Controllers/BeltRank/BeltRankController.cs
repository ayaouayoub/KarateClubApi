using KarateClub.Api.Controllers.User.Requests;
using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.BeltRank;
using KarateClub.Application.Handlers.BeltRank.Queries;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Handlers.User;
using KarateClub.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KarateClub.Application.Handlers.BeltRank.Commands;
using KarateClub.Api.Controllers.BeltRank.Requests;

namespace KarateClub.Api.Controllers.BeltRank
{
    [Route("api/belt-ranks")]
    [ApiController]
    public class BeltRankController : ControllerBase
    {
        private readonly GetBeltRankHandler _getBeltRankHandler;
        private readonly GetBeltsHandler _getBeltsHandler;
        private readonly ChangeBeltTestFeesHandler _changeBeltTestFeesHandler;

        public BeltRankController(GetBeltRankHandler getBeltRankHandler, GetBeltsHandler getBeltsHandler, ChangeBeltTestFeesHandler changeBeltTestFeesHandler)
        {
            _getBeltRankHandler = getBeltRankHandler;
            _getBeltsHandler = getBeltsHandler;
            _changeBeltTestFeesHandler = changeBeltTestFeesHandler;
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

        [Authorize(Policy = Permissions.BeltRanks.Update)]
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateBeltTestFees(int id, [FromBody] ChangeBeltTestFeesRequest request)
        {
            await _changeBeltTestFeesHandler.ExecuteAsync(new ChangeBeltTestFeesCommand(id, request.TestFees));
            return NoContent();
        }
    }
}
