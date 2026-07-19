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
    public class GetMemberHandler
    {
        private readonly IMemberRepository _memberRepository;

        public GetMemberHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<MemberDetailsDto> ExecuteAsync(GetMemberQuery query)
        {
            var member = await _memberRepository.GetByIdAsync(query.MemberId) ?? throw new NotFoundException("Member not found.");

            return new MemberDetailsDto
            {
                Id = member.Id,
                EmergencyContactInfo = member.EmergencyContactInfo,
                IsActive = member.IsActive,
                BeltRankDto = new BeltRankDto
                {
                    Id = member.LastBeltRankID,
                    Name = member.BeltRank.Name,
                    TestFees = member.BeltRank.TestFees
                },
                PersonDto = new PersonDto
                {
                    Id = member.PersonId,
                    Name = member.Person.Name,
                    Address = member.Person.Address,
                    Email = member.Person.Email.Value
                }
            };
        }
    }
}
