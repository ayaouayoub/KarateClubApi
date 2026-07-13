using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.DTOs
{
    internal class UserDetialsDto
    {
        public int Id { get; set; }
        public string? Username { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PersonId { get; set; }
        public PersonDto? PersonDto { get; set; }
        public IReadOnlyCollection<PermissionDto>? Permissions { get; set; }
    }
}
