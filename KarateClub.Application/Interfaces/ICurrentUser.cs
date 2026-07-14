using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.Interfaces
{
    public interface ICurrentUser
    {
        User User { get; }

        int UserId { get; }

        string Username { get; }

        bool IsSuperAdmin { get; }

        bool IsActive { get; }

        IReadOnlyCollection<string> Permissions { get; }
    }
}
