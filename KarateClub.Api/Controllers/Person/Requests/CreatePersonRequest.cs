namespace KarateClub.Api.Controllers.Person.Requests
{
    public class CreatePersonRequest
    {
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? Email { get; set; }
    }
}
