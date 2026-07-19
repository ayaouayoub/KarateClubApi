using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Handlers.Instructor.Queries;
using KarateClub.Application.Handlers.Member.Queries;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Member
{
    public class GetMembersHandler
    {
        private readonly IMemberRepository _memberRepository;

        public GetMembersHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IEnumerable<MemberDto>> ExecuteAsync(GetMembersQuery query)
        {
            var members = await _memberRepository.GetMembersAsync();

            return members.Select(m => new MemberDto
            {
                Id = m.Id,
                LastBeltRankID = m.LastBeltRankID,
                IsActive = m.IsActive,
                PersonID = m.PersonId,
                EmergencyContactInfo = m.EmergencyContactInfo
            });
        }
    }
}
