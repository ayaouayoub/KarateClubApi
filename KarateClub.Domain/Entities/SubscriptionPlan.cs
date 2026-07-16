using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Domain.Entities
{
    public class SubscriptionPlan
    {
        public int Id { get; }
        public string Name { get; } = null!;
        public int DurationInMonths { get; }
        public decimal Fees { get; }
        public bool IsActive { get; private set; }

        public SubscriptionPlan(int id, string name, int durationInMonths, decimal fees, bool isActive = true)
        {
            _ValidateName(name);
            _ValidateFees(fees);
            _ValidateDurationInMonths(durationInMonths);

            Id = id;
            Name = name;
            DurationInMonths = durationInMonths;
            Fees = fees;
            IsActive = isActive;
        }

        public static SubscriptionPlan Create(string name, int durationInMonths, decimal fees)
        {
            return new SubscriptionPlan(0, name, durationInMonths, fees);
        }

        public static SubscriptionPlan Load(int id, string name, int durationInMonths, decimal fees, bool isActive)
        {
            return new SubscriptionPlan(id, name, durationInMonths, fees, isActive);
        }

        public void Activate()
        {
            if (IsActive)
                throw new DomainException("The plan is already active.");

            IsActive = true;
        }

        public void Deactivate()
        {
            if (!IsActive)
                throw new DomainException("The plan is already inactive.");

            IsActive = false;
        }

        private void _ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name)) throw new DomainException("Subscription plan name cannot be null or empty");
        }

        private void _ValidateFees(decimal fees)
        {
            if (fees < 0) throw new DomainException("Subscription plan fees cannot be nigative");
        }

        private void _ValidateDurationInMonths(int DurationInMonths)
        {
            if (DurationInMonths < 0) throw new DomainException("Duration in months cannot be nigative");
            if (DurationInMonths > 12) throw new DomainException("Duration in months cannot be greater than 12");
        }
    }
}
