using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.DTOs
{
    public record MemberDetailsDto
    {
        public int Id { get; set; }
        public PersonDto PersonDto { get; set; } = null!;
        public string? EmergencyContactInfo { get; set; }
        public bool IsActive { get; set; }
        public BeltRankDto BeltRankDto { get; set; } = null!;
    }
}
