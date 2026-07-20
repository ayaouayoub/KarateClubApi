using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Domain.Entities;

namespace KarateClub.Application.Interfaces.Repositories
{
    public interface IMemberInstructorRepository
    {
        Task<IReadOnlyList<MemberInstructor>> GetByMemberIdAsync(int memberId);

        Task<IReadOnlyList<MemberInstructor>> GetByInstructorIdAsync(int instructorId);
    }
}
