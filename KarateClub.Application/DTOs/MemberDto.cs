using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.DTOs
{
    public record MemberDto
    {
        public int Id { get; set; }
        public int PersonID { get; set; }
        public string? EmergencyContactInfo { get; set; }
        public bool IsActive { get; set; }
        public int LastBeltRankID { get; set; }
    }
}
