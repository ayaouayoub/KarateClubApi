using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.DTOs
{
    public record MemberInstructorsDto
    {
        public int InstructorId { get; set; }
        public string InstructorName { get; set; } = null!;
        public DateTime AssignDate { get; set; }
    }
}
