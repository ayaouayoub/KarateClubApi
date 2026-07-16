namespace KarateClub.Api.Controllers.SubscriptionPlan.Requests
{
    public class CreateSubscriptionPlanRequest
    {
        public string Name { get; set; } = null!;
        public int DurationInMonths { get; set; }
        public decimal Fees { get; set;  }
    }
}
