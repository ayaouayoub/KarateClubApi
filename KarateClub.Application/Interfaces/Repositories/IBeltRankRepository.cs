using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.Interfaces.Repositories
{
    public interface IBeltRankRepository
    {
        Task<BeltRank?> GetByIdAsync(int id);
        Task<List<BeltRank>> GetBeltsAsync();
        Task<bool> ChangeBeltTestFeesAsync(decimal NewTestFees);
    }
}
