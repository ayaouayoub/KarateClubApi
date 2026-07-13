using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Application.Interfaces;
using KarateClub.Application.Handlers.User.Queries;

namespace KarateClub.Application.Handlers.User
{
    public class GetUserHandler
    {
        private readonly IUserRepository _repo;

        public GetUserHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<UserDetialsDto> ExecuteAsync(GetUserByIdQuery query)
        {
            var user = await _repo.GetByIdAsync(query.UserId);

            if (user == null)
                throw new NotFoundException("User not found.");

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
                    Email = user.Person?.Email.Value,
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
