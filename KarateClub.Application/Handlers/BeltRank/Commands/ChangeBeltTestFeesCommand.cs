using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.Handlers.BeltRank.Commands
{
    public record ChangeBeltTestFeesCommand(int BeltId, decimal TestFees);
}
