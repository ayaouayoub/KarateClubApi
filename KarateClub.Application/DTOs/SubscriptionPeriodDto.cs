namespace KarateClub.Application.DTOs
{
    public record SubscriptionPeriodDto
    {
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalFees { get; set; }
        public int MemberId { get; set; }
        public int SubscriptionPlanId { get; set; }
    }
}
