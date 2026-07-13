using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.User.Commands;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Application.Interfaces;

namespace KarateClub.Application.Handlers.User
{
    public class LoginHandler
    {
        private readonly IUserRepository _repo;

        private readonly IEncryptionService _encryption;

        private readonly IJwtTokenGenerator _jwt;

        public LoginHandler(IUserRepository repo, IEncryptionService encryption, IJwtTokenGenerator jwt)
        {
            _repo = repo;
            _encryption = encryption;
            _jwt = jwt;
        }

        public async Task<LoginResponseDto> ExecuteAsync(LoginCommand command)
        {
            var user = await _repo.GetByUsernameAsync(command.Username);

            if (user == null || !_encryption.Verify(command.Password, user.PasswordHash)) throw new Exception("Invalid credentials.");

            return new LoginResponseDto
            {
                Token = _jwt.Generate(user),

                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    IsSuperAdmin = user.IsSuperAdmin,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    PersonId = user.PersonId
                }
            };
        }
    }
}