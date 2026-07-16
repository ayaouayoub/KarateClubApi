using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.SubscriptionPlan.Queries;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.SubscriptionPlan
{
    public class GetSubscriptionPlansHandler
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

        public GetSubscriptionPlansHandler(ISubscriptionPlanRepository subscriptionPlanRepository)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
        }

        public async Task<IEnumerable<SubscriptionPlanDto>> ExecuteAsync(GetSubscriptionPlansQuery query)
        {
            var plans = await _subscriptionPlanRepository.GetSubscriptionPlansAsync();

            return plans.Select(p => new SubscriptionPlanDto
            {
                Id = p.Id,
                Name = p.Name,
                Fees = p.Fees,
                DurationInMonths = p.DurationInMonths,
                IsActive = p.IsActive
            });
        }
    }
}
