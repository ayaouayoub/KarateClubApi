using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.SubscriptionPlan.Commands;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.SubscriptionPlan
{
    public class CreateSubscriptionPlanHandler
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

        public CreateSubscriptionPlanHandler(ISubscriptionPlanRepository subscriptionPlanRepository)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
        }

        public async Task<SubscriptionPlanDto> ExecuteAsync(CreateSubscriptionPlanCommand command)
        {
            int newPlanId = await _subscriptionPlanRepository.AddSubscriptionPlanAsync(_CreateSubscriptionPlan(command));
            return new SubscriptionPlanDto()
            {
                Id = newPlanId,
                Name = command.Name,
                Fees = command.Fees,
                DurationInMonths = command.DurationInMonths
            };
        }

        private Domain.Entities.SubscriptionPlan _CreateSubscriptionPlan(CreateSubscriptionPlanCommand command)
        {
            return Domain.Entities.SubscriptionPlan.Create(command.Name, command.DurationInMonths, command.Fees);
        }
    }
}
