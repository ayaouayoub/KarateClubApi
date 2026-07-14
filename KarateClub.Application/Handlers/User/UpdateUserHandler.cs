using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Application.Interfaces;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Application.Handlers.User
{
    public sealed class UpdateUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IEncryptionService _encryptionService;

        public UpdateUserHandler(
            IUserRepository userRepository,
            IPersonRepository personRepository,
            IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
            _encryptionService = encryptionService;
        }

        public async Task<UserDto> ExecuteAsync(UpdateUserCommand command)
        {
            if (await _userRepository.GetByUsernameAsync(command.Username) is not null) throw new DomainException("Username already exists.");

            Domain.Entities.User user = await _userRepository.GetByIdAsync(command.UserId) ?? throw new NotFoundException("User not found.");

            Domain.Entities.Person person = await _personRepository.GetPersonById(command.PersonId) ?? throw new NotFoundException("Person not found.");

            if (user.PersonId != command.PersonId && await _userRepository.GetUserByPersonIdAsync(command.PersonId) is not null) throw new DomainException("this persone is associated with another user");

            _ChangeUserObjectData(command, user, person);

            return await _SaveChanges(user);
        }

        private void _ChangeUserObjectData(UpdateUserCommand command, Domain.Entities.User user, Domain.Entities.Person person)
        {
            user.ChangeUsername(command.Username);

            if (!string.IsNullOrWhiteSpace(command.Password))
            {
                user.ChangePassword(_encryptionService.Hash(command.Password));
            }

            user.ChangePerson(person);

            if (command.IsActive)
                user.Activate();
            else
                user.Deactivate();

            if (!user.IsSuperAdmin && command.Permissions is not null)
            {
                user.SetPermissions(command.Permissions);
            }
        }

        private async Task<UserDto> _SaveChanges(Domain.Entities.User user)
        {
            await _userRepository.UpdateUserAsync(user);

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                IsActive = user.IsActive,
                IsSuperAdmin = user.IsSuperAdmin,
                CreatedAt = user.CreatedAt,
                PersonId = user.PersonId,
                Permissions = user.Permissions.Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Code = p.Code
                }).ToList()
            };
        }
    }
}

