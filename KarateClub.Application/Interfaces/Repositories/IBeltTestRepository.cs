using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.Handlers.BeltTest.Commands;

namespace KarateClub.Application.Interfaces.Repositories
{
    public interface IBeltTestRepository
    {
        Task<int> RegisterAsync(RegisterBeltTestCommand command);
    }
}
