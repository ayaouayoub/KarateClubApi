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
        int UserId { get; }

        string Username { get; }

        bool IsSuperAdmin { get; }

        bool IsActive { get; }

        IReadOnlyCollection<string> permissions { get; }
    }
}
