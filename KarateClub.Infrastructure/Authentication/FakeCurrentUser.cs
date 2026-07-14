using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.Interfaces;

namespace KarateClub.Infrastructure.Authentication
{
    public class FakeCurrentUser : ICurrentUser
    {
        public bool IsAuthenticated => true;

        public int UserId => 1;

        public string Username => "User1";

        public bool IsSuperAdmin => false;

        public bool IsActive => true;

        public IReadOnlyCollection<string> Permissions => new List<string>()
        { 
            "Members.View",
            "Members.Create",
            "Users.View",
            "Payments.View"
        };
    }
}
