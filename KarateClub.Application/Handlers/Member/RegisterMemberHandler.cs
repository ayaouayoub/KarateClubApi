using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Member.Commands;
using KarateClub.Application.Interfaces.Repositories;
using KarateClub.Domain.Exceptions;

namespace KarateClub.Application.Handlers.Member
{
    public class RegisterMemberHandler
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IPersonRepository _personRepository;
        private readonly IInstructorRepository _instructorRepository;
        private readonly ISubscriptionPlanRepository _subscriptionPlanRepository;

        public RegisterMemberHandler(IMemberRepository memberRepository,IPersonRepository personRepository, IInstructorRepository instructorRepository, ISubscriptionPlanRepository subscriptionPlanRepository)
        {
            _memberRepository = memberRepository;
            _personRepository = personRepository;
            _instructorRepository = instructorRepository;
            _subscriptionPlanRepository = subscriptionPlanRepository;
        }

        public async Task<int> ExecuteAsync(RegisterMemberCommand command)
        {
            await _ValidateCommandAsync(command);
            return await _memberRepository.RegisterAsync(command);
        }

        private async Task _ValidateCommandAsync(RegisterMemberCommand command)
        {
            ArgumentNullException.ThrowIfNull(command);
            var personTask = _ValidatePersonAsync(command.PersonId);
            var memberTask = _ValidateMemberAsync(command.PersonId);
            var planTask = _ValidatePlanAsync(command);
            var instructorTask = _ValidateInstructorAsync(command);
            await Task.WhenAll(personTask,memberTask,planTask,instructorTask);
        }

        private async Task _ValidatePersonAsync(int PersonId)
        {
            if (await _personRepository.GetPersonByIdAsync(PersonId) is null)
                throw new NotFoundException("Person not found.");
        }

        private async Task _ValidateMemberAsync(int PersonId)
        {
            if (await _memberRepository.GetByPersonIdAsync(PersonId) is not null)
                throw new DomainException("Person is already registered as a member.");
        }

        private async Task _ValidatePlanAsync(RegisterMemberCommand command)
        {
            var plan = await _subscriptionPlanRepository.GetByIdAsync(command.SubscriptionPlanId);

            if (plan is null)
                throw new DomainException("Subscription plan not found.");

            if (!plan.IsActive)
                throw new DomainException("Subscription plan is inactive.");

            if (command.PaidAmount <= 0)
                throw new DomainException("Paid amount must be greater than zero.");

            if (command.PaidAmount > plan.Fees)
                throw new DomainException("Paid amount cannot exceed subscription fees.");
        }

        private async Task _ValidateInstructorAsync(RegisterMemberCommand command)
        {
            if (command.InstructorIds is null || command.InstructorIds.Count == 0)
                throw new DomainException("At least one instructor must be assigned.");

            if (command.InstructorIds.Distinct().Count() != command.InstructorIds.Count)
                throw new DomainException("Duplicate instructors are not allowed.");

            foreach (int instructorId in command.InstructorIds)
            {
                var instructor = await _instructorRepository.GetByIdAsync(instructorId);

                if (instructor is null)
                    throw new DomainException($"Instructor ({instructorId}) was not found.");

                if (!instructor.IsActive)
                    throw new DomainException($"Instructor ({instructorId}) is inactive.");

                if (instructor.PersonId == command.PersonId)
                    throw new DomainException("A member cannot be assigned to himself as an instructor.");
            }
        }
    }
}
