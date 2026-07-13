using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Application.Interfaces;
using KarateClub.Application.DTOs;
using System.Security.AccessControl;

namespace KarateClub.Application.Handlers.User
{
    public class GetUsersHandler
    {
        private readonly IUserRepository _repo;

        public GetUsersHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<UserDto>> ExecuteAsync()
        {
            var users = await _repo.GetUsersAsync();

            var usersDto = new List<UserDto>();

            foreach (var user in users)
            {
                usersDto.Add
                (
                    new UserDto
                    {
                        Id = user.Id,
                        Username = user.Username,
                        IsSuperAdmin = user.IsSuperAdmin,
                        IsActive = user.IsActive,
                        PersonId = user.PersonId,
                        CreatedAt = user.CreatedAt
                    }
                );
            }

            return usersDto;
        }
    }
}
