namespace KarateClub.Api.Controllers.Member.Requests
{
    public record RegisterMemberRequest
    {
        public int PersonId { get; init; }

        public string EmergencyContactInfo { get; init; } = "";

        public int SubscriptionPlanId { get; init; }

        public decimal PaidAmount { get; init; }

        public List<int> InstructorIds { get; init; } = [];
    }
}
