using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Exceptions;
using KarateClub.Domain.ValueObjs;

namespace KarateClub.Domain.Entities
{
    public class SubscriptionPeriod
    {
        public int Id { get; }
        public Period Period { get; } = null!;
        public decimal TotalFees { get; }
        public int MemberId { get; }
        public Member? Member { get; } = null!;
        public int SubscriptionPlanId { get; }
        public SubscriptionPlan? SubscriptionPlan { get; } = null!;

        private SubscriptionPeriod(int id, Period period, decimal totalFees, int memberId, Member? member, int subscriptionPlanId, SubscriptionPlan? subscriptionPlan)
        {
            if (totalFees < 0) throw new DomainException("Subscription period total fees cannot be nigative");
            Id = id;
            Period = period;
            TotalFees = totalFees;
            MemberId = memberId;
            Member = member;
            SubscriptionPlanId = subscriptionPlanId;
            SubscriptionPlan = subscriptionPlan;
        }

        public static SubscriptionPeriod Create(Period period, decimal totalFees, Member member, SubscriptionPlan subscriptionPlan)
        {
            return new SubscriptionPeriod(0, period, totalFees, member.Id, member, subscriptionPlan.Id, subscriptionPlan);
        }

        public static SubscriptionPeriod Load(int id, Period period, decimal totalFees, int memberId, int subscriptionPlanId)
        {
            return new SubscriptionPeriod(id, period, totalFees, memberId, null, subscriptionPlanId, null);
        }
    }
}
