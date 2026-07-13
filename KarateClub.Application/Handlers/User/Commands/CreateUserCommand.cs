using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;

namespace KarateClub.Application.Handlers.User.Commands
{
    public class CreateUserCommand
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool IsSuperAdmin { get; set; }
        public int PersonId { get; set; }
        public IReadOnlyCollection<int>? Permissions { get; set; }
    }
}
