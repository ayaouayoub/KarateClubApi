using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Application.Interfaces;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Application.Handlers.User
{
    public class ChangeMyPasswordHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUser _currentUser;
        private readonly IEncryptionService _encryptionService;

        public ChangeMyPasswordHandler(IUserRepository userRepository, ICurrentUser currentUser, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _currentUser = currentUser;
            _encryptionService = encryptionService;
        }

        public async Task ExecuteAsync(ChangeMyPasswordCommand command)
        {
            var user = await _userRepository.GetByIdAsync(_currentUser.UserId) ?? throw new NotFoundException("User not found.");

            if (!_encryptionService.Verify(command.CurrentPassword, user.PasswordHash))
                throw new DomainException("Current password is incorrect.");

            user.ChangePassword(_encryptionService.Hash(command.NewPassword));

            if (!await _userRepository.ChangeMyPasswordAsync(user.Id, user.PasswordHash))
            {
                throw new Exception("failed to update your password");
            }
        }
    }
}
