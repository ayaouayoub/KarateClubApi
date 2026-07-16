using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.Interfaces.Repositories
{
    public interface ISubscriptionPlanRepository
    {
        Task<SubscriptionPlan?> GetByIdAsync(int id);
        Task<IEnumerable<SubscriptionPlan>> GetSubscriptionPlansAsync();
        Task<int> AddSubscriptionPlanAsync(SubscriptionPlan subscriptionPlan);
        Task<bool> DeactivatePlanAsync(int id);
        Task<bool> GetPlanByUserIdAsync(int userId);
    }
}
