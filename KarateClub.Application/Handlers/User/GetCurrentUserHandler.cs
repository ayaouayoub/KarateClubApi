using KarateClub.Application.DTOs;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Application.Interfaces;

namespace KarateClub.Application.Handlers.User
{
    public class GetCurrentUserHandler
    {
        private readonly IUserRepository _repo;

        private readonly ICurrentUser _currentUser;

        public GetCurrentUserHandler(IUserRepository repo, ICurrentUser currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        public async Task<UserDetialsDto> ExecuteAsync()
        {
            var user = await _repo.GetByIdAsync(_currentUser.UserId);

            if (user == null)
                throw new Exception("User not found.");

            return new UserDetialsDto
            {
                Id = user.Id,
                Username = user.Username,
                IsSuperAdmin = user.IsSuperAdmin,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                PersonId = user.PersonId,

                PersonDto = new PersonDto
                {
                    Id = user.PersonId,
                    Address = user.Person?.Address,
                    Email = user.Person?.Email,
                    Name = user.Person?.Name
                },

                Permissions = user.Permissions?.Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Code = p.Code
                }).ToList()
            };
        }
    }
}
