using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Application.Interfaces;
using KarateClub.Application.Handlers.User.Commands;

namespace KarateClub.Application.Handlers.User
{
    public class DeactivateUserHandler
    {
        private readonly IUserRepository _repo;

        public DeactivateUserHandler(IUserRepository repo)
        {
            _repo = repo;
        }

        public async Task ExecuteAsync(DeactivateUserCommand command)
        {
            var user = await _repo.GetByIdAsync(command.userId);

            if (user is null)
                throw new Exception("User not found.");

            user.Deactivate();

            if (!await _repo.DeactivateUserAsync(command.userId))
                throw new Exception("Failed to deactivate user.");
        }
    }
}
