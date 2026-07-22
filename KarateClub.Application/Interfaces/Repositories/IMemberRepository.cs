using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.Handlers.Member.Commands;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.Interfaces.Repositories
{
    public interface IMemberRepository
    {
        Task<Member?> GetByIdAsync(int id);
        Task<Member?> GetByPersonIdAsync(int personId);
        Task<List<Member>> GetMembersAsync();
        Task<bool> DeactivateMemberAsync(int id);
        Task<bool> ActivateMemberAsync(int id);
        Task<bool> UpdateCurrentBletRankAsync(int memberId, int beltRankId);
        Task<bool> UpdateEmergencyContactInfoAsync(int memberId, string emergencyContactInfo);
        Task<int> RegisterAsync(RegisterMemberCommand command);
    }
}
