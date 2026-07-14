using KarateClub.Application.DTOs;

namespace KarateClub.Api.Controllers.User.Requests
{
    public sealed class UpdateUserRequest
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public bool IsActive { get; set; }

        public int PersonId { get; set; }

        public IReadOnlyCollection<PermissionDto>? Permissions { get; set; }
    }
}
