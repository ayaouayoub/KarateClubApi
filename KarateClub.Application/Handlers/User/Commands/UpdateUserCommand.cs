using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.Handlers.User.Commands
{
    public sealed class UpdateUserCommand
    {
        public int UserId { get; set; }

        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public bool IsActive { get; set; }

        public int PersonId { get; set; }

        public IReadOnlyCollection<Permission>? Permissions { get; set; }
    }
}
