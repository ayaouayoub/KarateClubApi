using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.SubscriptionPlan.Commands;
using KarateClub.Application.Interfaces;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Application.Handlers.SubscriptionPlan
{
    public class DeactivatePlanHandler
    {
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;
        private readonly ICurrentUser _currentUser;

        public DeactivatePlanHandler(ISubscriptionPlanRepository subscriptionPlanRepository, ICurrentUser currentUser)
        {
            _subscriptionPlanRepository = subscriptionPlanRepository;
            _currentUser = currentUser;
        }

        public async Task ExecuteAsync(DeactivatePlanCommand command)
        {
            if (!_currentUser.IsSuperAdmin) throw new DomainException("You have to be super admin to deactivate a subscription plan");

            var plan = await _subscriptionPlanRepository.GetByIdAsync(command.PlandId);

            if (plan is null) throw new NotFoundException("Subscription plan not found.");

            plan.Deactivate();

            if (!await _subscriptionPlanRepository.DeactivatePlanAsync(command.PlandId)) throw new Exception($"Failed to deactivate user subscription plan with id {command.PlandId}.");
        }
    }
}
