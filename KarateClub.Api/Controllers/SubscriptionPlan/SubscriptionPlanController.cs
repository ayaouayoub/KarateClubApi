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
    [Route("api/subscription-plans")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly GetSubscriptionPlanHandler _getSubscriptionPlanHandler;
        private readonly GetSubscriptionPlansHandler _getSubscriptionPlansHandler;

        public SubscriptionPlanController(GetSubscriptionPlanHandler getSubscriptionPlanHandler, GetSubscriptionPlansHandler getSubscriptionPlansHandler)
        {
            _getSubscriptionPlanHandler = getSubscriptionPlanHandler;
            _getSubscriptionPlansHandler = getSubscriptionPlansHandler;
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

        [Authorize]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<SubscriptionPlanDto>>> GetSubscriptionPlans()
        {
            return Ok(await _getSubscriptionPlansHandler.ExecuteAsync(new GetSubscriptionPlansQuery()));
        }
    }
}
