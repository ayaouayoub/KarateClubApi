using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.Handlers.Member.Commands
{
    public record RegisterMemberCommand
    {
        public int PersonId { get; init; }

        public string EmergencyContactInfo { get; init; } = "";

        public int SubscriptionPlanId { get; init; }

        public decimal PaidAmount { get; init; }

        public List<int> InstructorIds { get; init; } = [];
    }
}
