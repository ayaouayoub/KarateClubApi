using KarateClub.Application.DTOs;

namespace KarateClub.Api.Controllers.User.Requests
{
    public sealed class CreateUserRequest
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int PersonId { get; set; }
        public IReadOnlyCollection<PermissionDto>? Permissions { get; set; }
    }
}
