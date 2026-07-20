using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Instructor.Queries;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Instructor
{
    public class GetInstructorMembersHandler
    {
        private readonly IMemberInstructorRepository _memberInstructorRepository;
        private readonly IInstructorRepository _instructorRepository;

        public GetInstructorMembersHandler(IMemberInstructorRepository memberInstructorRepository, IInstructorRepository instructorRepository)
        {
            _memberInstructorRepository = memberInstructorRepository;
            _instructorRepository = instructorRepository;
        }

        public async Task<IEnumerable<InstructorMembersDto>> ExecuteAsync(GetInstructorMembersQuery query)
        {
            var instructor = await _instructorRepository.GetByIdAsync(query.InstructorId) ?? throw new NotFoundException("Instructor not found.");
            var instructorMembers = await _memberInstructorRepository.GetByInstructorIdAsync(instructor.Id);
            return instructorMembers.Select(im => new InstructorMembersDto
            {
                MemberId = im.MemberId,
                AssignDate = im.AssignDate,
                MemberName = im.Member.Person.Name
            });
        }
    }
}
