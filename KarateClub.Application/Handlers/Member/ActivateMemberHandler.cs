using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Instructor.Commands;
using KarateClub.Application.Handlers.Member.Commands;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Member
{
    public class ActivateMemberHandler
    {
        private readonly IMemberRepository _memberRepository;

        public ActivateMemberHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task ExecuteAsync(ActivateMemberCommand command)
        {
            var member = await _memberRepository.GetByIdAsync(command.MemberId) ?? throw new NotFoundException("Member not found.");

            member.Activate();

            if (!await _memberRepository.ActivateMemberAsync(command.MemberId)) throw new Exception("Fialed to Activate member.");
        }
    }
}
