namespace KarateClub.Api.Controllers.Person.Requests
{
    public class UpdatePersonRequest
    {
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? Email { get; set; }
    }
}
