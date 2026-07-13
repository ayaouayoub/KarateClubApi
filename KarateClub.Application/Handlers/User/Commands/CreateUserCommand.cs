using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.Handlers.User.Commands
{
    public class CreateUserCommand
    {
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public int PersonId { get; set; }
        public IReadOnlyCollection<Permission>? Permissions { get; set; }
    }
}
