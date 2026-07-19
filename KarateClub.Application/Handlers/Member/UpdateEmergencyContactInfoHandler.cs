using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Member.Commands;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Member
{
    public class UpdateEmergencyContactInfoHandler
    {
        private readonly IMemberRepository _memberRepository;

        public UpdateEmergencyContactInfoHandler(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task ExecuteAsync(UpdateEmergencyContactInfoCommand command)
        {
            var member = await _memberRepository.GetByIdAsync(command.MemberId) ?? throw new NotFoundException("Member not found.");

            member.ChangeEmergencyContactInfo(command.EmergencyContactInfo);

            if (!await _memberRepository.UpdateEmergencyContactInfoAsync(command.MemberId, command.EmergencyContactInfo)) throw new Exception("Fialed to update member's emergency contact info.");
        }
    }
}
