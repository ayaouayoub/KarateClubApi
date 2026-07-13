using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.Handlers.User.Commands
{
    public record LoginCommand(string Username, string Password);
}
