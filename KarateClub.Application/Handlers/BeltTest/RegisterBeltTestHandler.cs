using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.BeltTest.Commands;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Application.Handlers.BeltTest
{
    public class RegisterBeltTestHandler
    {
        private readonly IBeltTestRepository _beltTestRepository;
        private readonly IMemberRepository _memberRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly IBeltRankRepository _beltRankRepository;
        private readonly ISubscriptionPeriodRepository _subscriptionPeriodRepository;

        public RegisterBeltTestHandler(IBeltTestRepository beltTestRepository,IMemberRepository memberRepository, IInstructorRepository instructorRepository, IBeltRankRepository beltRankRepository, ISubscriptionPeriodRepository subscriptionPeriodRepository)
        {
            _beltTestRepository = beltTestRepository;
            _memberRepository = memberRepository;
            _instructorRepository = instructorRepository;
            _beltRankRepository = beltRankRepository;
            _subscriptionPeriodRepository = subscriptionPeriodRepository;
        }

        public async Task<int> ExecuteAsync(RegisterBeltTestCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);
            await _ValidateCommand(command);
            return await _beltTestRepository.RegisterAsync(command);
        }

        private async Task _ValidateCommand(RegisterBeltTestCommand command)
        {
            var member = await _ValidateMember(command);
            await _ValidatePeriod(command);
            await _Validateinstructor(member, command);
            await _ValidateBeltRank(command);
        }

        private async Task<Domain.Entities.Member> _ValidateMember(RegisterBeltTestCommand command)
        {
            var member = await _memberRepository.GetByIdAsync(command.MemberId) ?? throw new NotFoundException("Member not found.");
            if (!member.IsActive) throw new DomainException("Member is inactive.");
            return member;
        }

        private async Task<Domain.Entities.SubscriptionPeriod> _ValidatePeriod(RegisterBeltTestCommand command)
        {
            return await _subscriptionPeriodRepository.GetCurrentPeriodAsync(command.MemberId) ?? throw new DomainException("Member has no active subscription.");
        }

        private async Task _Validateinstructor(Domain.Entities.Member member, RegisterBeltTestCommand command)
        {
            var instructor = await _instructorRepository.GetByIdAsync(command.TestedByInstructorId) ?? throw new NotFoundException("Instructor not found.");

            if (!instructor.IsActive) throw new DomainException("Instructor is inactive.");

            if (instructor.PersonId == member.PersonId) throw new DomainException("Instructor cannot test himself.");
        }

        private async Task<Domain.Entities.BeltRank> _ValidateBeltRank(RegisterBeltTestCommand command)
        {
            return await _beltRankRepository.GetByIdAsync(command.RankId) ?? throw new NotFoundException("Belt rank not found.");
        }
    }
}
