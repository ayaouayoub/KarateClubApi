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
    public class DeactivateMemberHandler
    {
        private readonly IMemberRepository _memberRepository;

        public DeactivateMemberHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task ExecuteAsync(DeactivateMemberCommand command)
        {
            var instructor = await _memberRepository.GetByIdAsync(command.MmeberId) ?? throw new NotFoundException("Member not found.");

            instructor.Deactivate();

            if (!await _memberRepository.DeactivateMemberAsync(command.MmeberId)) throw new Exception("Fialed to deactivate member.");
        }
    }
}
