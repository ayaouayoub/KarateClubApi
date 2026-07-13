using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.Interfaces
{
    public interface IJwtTokenGenerator
    {
        string Generate(User user);
    }
}
