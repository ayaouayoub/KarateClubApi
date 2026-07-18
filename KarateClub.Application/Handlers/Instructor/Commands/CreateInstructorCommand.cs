using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.Handlers.Instructor.Commands
{
    public record CreateInstructorCommand
    {
        public int PersonId { get; set; }
        public string Qualification { get; set; } = null!;
        public int BeltRankID { get; set; }
    }
}
