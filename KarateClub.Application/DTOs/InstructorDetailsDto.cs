using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.DTOs
{
    public class InstructorDetailsDto
    {
        public int Id { get; set; }
        public PersonDto PersonDto { get; set; } = null!;
        public string Qualification { get; set; } = null!;
        public DateTime HireDate { get; set; } 
        public bool IsActive { get; set; }
        public BeltRankDto BeltRankDto { get; set; } = null!;
    }
}
