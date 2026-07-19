using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.Handlers.Member.Commands
{
    public record UpdateEmergencyContactInfoCommand(int MemberId, string EmergencyContactInfo);
}
