using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.User.Queries;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.User
{
    public class GetUserByPersonIdHandler
    {
        private readonly IUserRepository _repo;

        public GetUserByPersonIdHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserWithoutPermissionsDto> ExecuteAsync(GetUserByPersonIdQuery query)
        {
            var user = await _repo.GetByIdAsync(query.PersonId);

            if (user == null)
                throw new NotFoundException("User not found.");

            return new UserWithoutPermissionsDto()
            {
                Id = user.Id,
                Username = user.Username,
                IsSuperAdmin = user.IsSuperAdmin,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                PersonId = user.PersonId,
            };
        }
    }
}
