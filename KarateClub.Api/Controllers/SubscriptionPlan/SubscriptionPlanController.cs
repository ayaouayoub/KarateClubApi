using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.BeltRank.Queries;
using KarateClub.Application.Handlers.BeltRank;
using KarateClub.Application.Handlers.SubscriptionPlan;
using KarateClub.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KarateClub.Application.Handlers.SubscriptionPlan.Queries;

namespace KarateClub.Api.Controllers.SubscriptionPlan
{
    [Route("api/subscription-plan")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly GetSubscriptionPlanHandler _getSubscriptionPlanHandler;

        public SubscriptionPlanController(GetSubscriptionPlanHandler getSubscriptionPlanHandler)
        {
            _getSubscriptionPlanHandler = getSubscriptionPlanHandler;
        }

        [Authorize]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SubscriptionPlanDto>> GetSubscriptionPlanById(int id)
        {
            return Ok(await _getSubscriptionPlanHandler.ExecuteAsync(new GetSubscriptionPlanQuery(id)));
        }
    }
}
