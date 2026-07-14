using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.Interfaces;
using KarateClub.Domain.Entities;

namespace KarateClub.Infrastructure.Authentication
{
    public class FakeCurrentUser : ICurrentUser
    {
        public User User => Domain.Entities.User.Load(1, "Admin", "Admin123", true, true, DateTime.UtcNow, 1);

        public bool IsAuthenticated => true;

        public int UserId => User.Id;

        public string Username => User.Username;

        public bool IsSuperAdmin => User.IsSuperAdmin;

        public bool IsActive => User.IsActive;

        public IReadOnlyCollection<string> Permissions => new List<string>()
        { 
            "Members.View",
            "Members.Create",
            "Users.View",
            "Payments.View"
        };
    }
}
