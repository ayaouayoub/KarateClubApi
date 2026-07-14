using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Interfaces;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Application.Handlers.User
{
    public class ChangeMyUsernameHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;

        public ChangeMyUsernameHandler(IUserRepository userRepository, ICurrentUser currentUser)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
        }

        public async Task ExecuteAsync(UpdateMyUsernameCommand command)
        {
            var user = await _userRepository.GetByIdAsync(_currentUser.UserId) ?? throw new NotFoundException("User not found.");

            if (await _userRepository.GetByUsernameAsync(command.Username) is not null)
                throw new DomainException("Username already exists.");

            user.ChangeUsername(command.Username);

            if (!await _userRepository.ChangeMyUsernameAsync(user.Id, command.Username))
            {
                throw new Exception("failed to update your username");
            }
        }
    }
}
