using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Member.Queries;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Member
{
    public class GetMemberByPersonIdHandler
    {
        private readonly IMemberRepository _memberRepository;

        public GetMemberByPersonIdHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<MemberDto> ExecuteAsync(GetMemberByPersonIdQuery query)
        {
            var member = await _memberRepository.GetByPersonIdAsync(query.PersonId) ?? throw new NotFoundException("Member not found.");

            return new MemberDto
            {
                Id = member.Id,
                EmergencyContactInfo = member.EmergencyContactInfo,
                IsActive = member.IsActive,
                LastBeltRankID = member.LastBeltRankID,
                PersonID = member.PersonId
            };
        }
    }
}
