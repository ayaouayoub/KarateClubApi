using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Member.Queries;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Member
{
    public class GetMemberInstructorsHandler
    {
        private readonly IMemberInstructorRepository _memberInstructorRepository;
        private readonly IMemberRepository _memberRepository;

        public GetMemberInstructorsHandler(IMemberInstructorRepository memberInstructorRepository, IMemberRepository memberRepository)
        {
            _memberInstructorRepository = memberInstructorRepository;
            _memberRepository = memberRepository;
        }

        public async Task<IEnumerable<MemberInstructorsDto>> ExecuteAsync(GetMemberInstructorsQuery query)
        {
            var member = await _memberRepository.GetByIdAsync(query.MemberId) ?? throw new NotFoundException("Member not found.");
            var instructorMembers = await _memberInstructorRepository.GetByMemberIdAsync(member.Id);
            return instructorMembers.Select(im => new MemberInstructorsDto
            {
                InstructorId = im.InstructorId,
                AssignDate = im.AssignDate,
                InstructorName = im.Instructor.Person.Name
            });
        }
    }
}
