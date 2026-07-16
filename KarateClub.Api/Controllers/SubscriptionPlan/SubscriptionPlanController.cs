using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.BeltRank.Queries;
using KarateClub.Application.Handlers.BeltRank;
using KarateClub.Application.Handlers.SubscriptionPlan;
using KarateClub.Application.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using KarateClub.Application.Handlers.SubscriptionPlan.Queries;
using KarateClub.Api.Controllers.Person.Requests;
using KarateClub.Application.Handlers.Person.Commnds;
using KarateClub.Application.Handlers.Person;
using KarateClub.Api.Controllers.SubscriptionPlan.Requests;
using KarateClub.Application.Handlers.SubscriptionPlan.Commands;

namespace KarateClub.Api.Controllers.SubscriptionPlan
{
    [Route("api/subscription-plans")]
    [ApiController]
    public class SubscriptionPlanController : ControllerBase
    {
        private readonly GetSubscriptionPlanHandler _getSubscriptionPlanHandler;
        private readonly GetSubscriptionPlansHandler _getSubscriptionPlansHandler;
        private readonly CreateSubscriptionPlanHandler _createSubscriptionPlanHandler;

        public SubscriptionPlanController(GetSubscriptionPlanHandler getSubscriptionPlanHandler, GetSubscriptionPlansHandler getSubscriptionPlansHandler, CreateSubscriptionPlanHandler createSubscriptionPlanHandler)
        {
            _getSubscriptionPlanHandler = getSubscriptionPlanHandler;
            _getSubscriptionPlansHandler = getSubscriptionPlansHandler;
            _createSubscriptionPlanHandler = createSubscriptionPlanHandler;
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

        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(SubscriptionPlanDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SubscriptionPlanDto>> AddNewSubscriptionPlan(CreateSubscriptionPlanRequest request)
        {
            var command = new CreateSubscriptionPlanCommand
            {
                Name = request.Name,
                DurationInMonths = request.DurationInMonths,
                Fees = request.Fees
            };

            SubscriptionPlanDto plan = await _createSubscriptionPlanHandler.ExecuteAsync(command);

            return CreatedAtAction(nameof(GetSubscriptionPlanById), new { id = plan.Id }, plan);
        }
    }
}
