using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Member.Commands;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Member
{
    public class UpdateMemberCurrentBletRankHandler
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IBeltRankRepository _beltRankRepository;

        public UpdateMemberCurrentBletRankHandler(IMemberRepository memberRepository, IBeltRankRepository beltRankRepository)
        {
            _memberRepository = memberRepository;
            _beltRankRepository = beltRankRepository;
        }

        public async Task ExecuteAsync(UpdateMemberCurrentBletRankCommand command)
        {
            var member = await _memberRepository.GetByIdAsync(command.MemberId) ?? throw new NotFoundException("Member not found.");
            var beltRank = await _beltRankRepository.GetByIdAsync(command.BeltRankId) ?? throw new NotFoundException("Belt rank not found.");

            member.ChangeBeltRank(beltRank);

            if (!await _memberRepository.UpdateCurrentBletRankAsync(command.MemberId, command.BeltRankId)) throw new Exception("Fialed to update member's blet rank.");
        }
    }
}
