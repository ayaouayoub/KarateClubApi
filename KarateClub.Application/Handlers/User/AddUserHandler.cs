using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Interfaces;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Application.Security;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Application.Handlers.User
{
    public class AddUserHandler
    {
        private readonly IUserRepository _userRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IEncryptionService _encryptionService;


        public AddUserHandler(IUserRepository userRepository, IPersonRepository personRepository, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _personRepository = personRepository;
            _encryptionService = encryptionService;
        }

        public async Task<UserDto> ExecuteAsync(CreateUserCommand command)
        {
            var user = CreateUser(command, await _GetPerson(command.PersonId));

            SetPermissions(command.Permissions, user);

            if (await _userRepository.GetUserByPersonIdAsync(command.PersonId) is not null)
            {
                throw new DomainException("this persone is associated with another user");
            }

            int newUserId = await _userRepository.AddUserAsync(user);

            return new UserDto()
            {
                Id = newUserId,
                Username = user.Username,
                CreatedAt = user.CreatedAt,
                IsActive = user.IsActive,
                IsSuperAdmin = user.IsSuperAdmin,
                Permissions = user.Permissions?.Select(p => new PermissionDto
                {
                    Id = p.Id,
                    Code = p.Code
                }).ToList(),
                PersonId = user.PersonId
            };
        }

        private async Task<Domain.Entities.Person> _GetPerson(int id)
        {
            var person = await _personRepository.GetPersonByIdAsync(id);
            if (person is null) throw new Exceptions.NotFoundException("Person not found");
            return person;
        }

        private Domain.Entities.User CreateUser(CreateUserCommand command, Domain.Entities.Person person)
        {
            return Domain.Entities.User.Create(command.Username, _encryptionService.Hash(command.Password), person);
        }

        private void SetPermissions(IReadOnlyCollection<Domain.Entities.Permission>? permissions, Domain.Entities.User user)
        {
            if (permissions is null)
            {
                return;
            }
            foreach (var permission in permissions) user.AddPermission(Domain.Entities.Permission.Load(permission.Id, permission.Code));
        }
    }
}
