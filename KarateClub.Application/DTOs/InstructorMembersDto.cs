using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.DTOs
{
    public record InstructorMembersDto
    {
        public int MemberId { get; set; }
        public string MemberName { get; set; } = null!;
        public DateTime AssignDate { get; set; }

    }
}
