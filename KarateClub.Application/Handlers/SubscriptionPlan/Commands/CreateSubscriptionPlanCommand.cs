using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.Handlers.SubscriptionPlan.Commands
{
    public class CreateSubscriptionPlanCommand
    {
        public string Name { get; set; } = null!;
        public int DurationInMonths { get; set; }
        public decimal Fees { get; set; }
    }
}
