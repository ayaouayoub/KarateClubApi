using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.BeltRank;
using KarateClub.Application.Handlers.BeltRank.Queries;
using KarateClub.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KarateClub.Api.Controllers.BeltRank
{
    [Route("api/beltrank")]
    [ApiController]
    public class BeltRankController : ControllerBase
    {
        private readonly GetBeltRankHandler _getBeltRankHandler;
        public BeltRankController(GetBeltRankHandler getBeltRankHandler)
        {
            _getBeltRankHandler = getBeltRankHandler;
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
    }
}
