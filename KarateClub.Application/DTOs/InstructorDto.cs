using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.DTOs
{
    public record InstructorDto
    {
        public int Id { get; set; }
        public int PersonID { get; set; }
        public string? Qualification { get; set; }
        public DateTime HireDate { get; set; }
        public bool IsActive { get; set; }
        public int CurrentBeltRankID { get; set; }
    }
}
