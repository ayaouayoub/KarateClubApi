using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.DTOs
{
    public class SubscriptionPlanDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int DurationInMonths { get; set; }
        public decimal Fees { get; set; }
        public bool IsActive { get; set; }
    }
}
