using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.SubscriptionPlan.Queries;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.SubscriptionPlan
{
    public class GetSubscriptionPlanHandler
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

        public GetSubscriptionPlanHandler(ISubscriptionPlanRepository subscriptionPlanRepository)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
        }

        public async Task<SubscriptionPlanDto> ExecuteAsync(GetSubscriptionPlanQuery getSubscriptionPlanQuery)
        {
            var plan = await _subscriptionPlanRepository.GetByIdAsync(getSubscriptionPlanQuery.SubscriptionPlanId) ?? throw new NotFoundException("Subscription plan not found.");

            return new SubscriptionPlanDto
            {
                Id = plan.Id,
                Name = plan.Name,
                DurationInMonths = plan.DurationInMonths,
                Fees = plan.Fees,
                IsActive = plan.IsActive
            };
        }
    }
}
