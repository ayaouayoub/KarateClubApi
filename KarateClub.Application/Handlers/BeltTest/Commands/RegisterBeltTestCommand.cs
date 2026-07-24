using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.Handlers.BeltTest.Commands
{
    public record RegisterBeltTestCommand
    {
        public int MemberId { get; set; }
        public int RankId { get; set; }
        public int TestedByInstructorId { get; set; }
    }
}
