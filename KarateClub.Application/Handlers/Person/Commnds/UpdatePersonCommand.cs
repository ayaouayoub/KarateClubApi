using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KarateClub.Application.Handlers.Person.Commnds
{
    public class UpdatePersonCommand
    {
        public int PersonId { get; set; }
        public string Name { get; set; } = null!;
        public string? Address { get; set; }
        public string? Email { get; set; }
    }
}
