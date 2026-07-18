namespace KarateClub.Api.Controllers.Instructor.Requests
{
    public record CreateInstructorRequest
    {
        public int PersonId { get; set; }
        public string Qualification { get; set; } = null!;
        public int BeltRankID { get; set; }
    }
}
