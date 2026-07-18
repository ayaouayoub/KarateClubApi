using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KarateClub.Application.DTOs;
using KarateClub.Application.Exceptions;
using KarateClub.Application.Handlers.Instructor.Queries;
using KarateClub.Application.Interfaces.Repositories;

namespace KarateClub.Application.Handlers.Instructor
{
    public class GetInstructorsHandler
    {
        private readonly IInstructorRepository _instructorRepository;

        public GetInstructorsHandler(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<IEnumerable<InstructorDto>> ExecuteAsync(GetInstructorsQuery query)
        {
            var instructors = await _instructorRepository.GetInstructorsAsync();

            return instructors.Select(i => new InstructorDto
            {
                Id = i.Id,
                CurrentBeltRankID = i.CurrentBeltRankID,
                HireDate = i.HireDate,
                IsActive = i.IsActive,
                PersonID = i.PersonId,
                Qualification = i.Qualification
            });
        }
    }
}
